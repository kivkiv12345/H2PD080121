using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SchemaClasses
{
    public abstract class DataController
    {
        // TODO Kevin: This seems like a terrible place to store an instance of the manager.
        public static DatabaseManager DBManager;

        private delegate string saveStringConverter(object input);

        /// <summary>
        /// This dictionary contains delegates which convert datatypes to formats acceptable when saving.
        /// </summary>
        private static Dictionary<Type, saveStringConverter> saveConversions = new Dictionary<Type, saveStringConverter>() {
            { typeof(DateTime), new saveStringConverter(delegate (object _date) {
                return ((DateTime)_date).ToString("yyyy-mm-dd");
            })},
            { typeof(Grades), new saveStringConverter(delegate (object _grade) {
                return ((Grades)_grade).ToString()[1..];
            })},
        };

        private static IEnumerable<(string, object)> getPrimaryKeys(DataModel instance)
        {
            // TODO Kevin: This will break when non primary key columns end with 'id'.
            PropertyInfo[] currProps = instance.GetType().GetProperties();
            IEnumerable<(string, object)> PKs = from field in currProps where field.Name.ToLower().EndsWith("id") select (field.Name, field.GetValue(instance, null));
            return PKs.Count() > 0 ? PKs : new (string, object)[] { (currProps.First().Name, currProps.First().GetValue(instance, null)) };
        }

        private static object[] _invalidBasetypes = new object[] { typeof(DataModel), null };

        /// <summary>
        /// Creates or updates a record in the database according to the provided DataModel.
        /// </summary>
        /// <param name="instance">The object to save</param>
        public static void Save(DataModel instance)
        {
            try
            {
                instance.validate();
            }
            catch (ValidationError e)
            {
                // TODO Kevin: Perhaps handle user feedback here.
                throw e;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine($"{instance.GetType().Name} does not contain a validator.");
            }

            string _convertSaveString(PropertyInfo prop)
            {
                var value = prop.GetValue(instance, null);
                try
                {
                    if (value is DataModel)
                    {
                        DataController.Save((DataModel)value);
                        return PersonController.getPrimaryKeys((DataModel)value).First().Item2.ToString();
                    }
                    return saveConversions[prop.PropertyType](value);
                }
                catch (KeyNotFoundException)
                {
                    return (value ?? "null").ToString();
                }
            }

            Type instanceType = instance.GetType();

            using (MySqlConnection conn = new MySqlConnection(DBManager.ConnectionString))
            {
                /// <summary>
                /// Saves a specific class to a specific table. Handles subclasses.
                /// </summary>
                /// <param name="currTupe">A subclass of DBModel, of which properties should be saved.</param>
                /// <returns>The properties that were saved.</returns>
                (string?, HashSet<string>) saveCurrentTable(Type currType)  // Method inspired by: https://stackoverflow.com/questions/8868119/find-all-parent-types-both-base-classes-and-interfaces
                {
                    // TODO Kevin: We might not want the connection to be passed as a parameter.

                    // We just attempted to save a nonexistent base class (most likely because we just walked to the top the inheritance hierarchy),
                    // hence no fields were saved. We therefore create and return an empty hashset, indicating to the previous recursion level,
                    // that all its found fields should be saved under its model.
                    if (currType == null)
                        return (null, new HashSet<string>());

                    // These are the fields that have already been handled. We need them to know which current fields to exclude.
                    (string PKValue, HashSet<string> savedFields) = saveCurrentTable(currType.BaseType);

                    PropertyInfo[] currentProperties = currType.GetProperties();

                    // These are the field names, followed by their values, that should be saved at this recursion level.
                    List<string> orderedFieldsNamesToSave = (from field in currentProperties where !savedFields.Contains(field.Name) && field.GetValue(instance, null) != null select field.Name).ToList();
                    List<string> orderedFieldValuesToSave = (from fieldName in orderedFieldsNamesToSave select _convertSaveString(currType.GetProperty(fieldName))).ToList();

                    if (!_invalidBasetypes.Contains(currType.BaseType) && PKValue != null)
                    {
                        orderedFieldsNamesToSave.Add(currType.BaseType.Name);
                        orderedFieldValuesToSave.Add(PKValue);
                    }

                    // The current model doesn't define any unsaved fields, so we don't save anything for it.
                    // TODO Kevin: This may not be the case for certain intermediate models where primary keys still need to be handled.
                    if (orderedFieldsNamesToSave.Count() == 0)
                        return (PKValue, savedFields);

                    // TODO Kevin: Thread-safe get max PK

                    // TODO Kevin: This seems clunky.
                    string PKColumn;
                    if (currType.BaseType == typeof(DataModel))
                    {
                        IEnumerable<string> PKColumns = from field in currentProperties where field.Name.ToLower().EndsWith("id") select field.Name;

                        if (PKColumns.Count() != 1 && typeof(DataModel) != instance.GetType().BaseType)
                            throw new Exception("Database models using inheritance must define exactly 1 explicit primary key column.");

                        try
                        {
                            PKColumn = PKColumns.First();
                        }
                        catch (InvalidOperationException)
                        {
                            // Assume the first property to be the primary key, when no more suitable column could be found.
                            PKColumn = currentProperties.First().Name;
                        }
                    }
                    else
                    {
                        string baseName = currType.BaseType.Name;
                        PKColumn = baseName[0].ToString().ToLower() + baseName[1..];  // Converting from pascal case to camel case.
                    }

                    new MySqlCommand($"USE {DatabaseManager.databaseName}", conn).ExecuteNonQuery();

                    if (instance.isSaved)
                    {
                        IEnumerable<string> fieldAndValue = orderedFieldsNamesToSave.Zip(orderedFieldValuesToSave, (a, b) => a + " = " + b);
                        string sql = $"UPDATE {currType.Name} SET {string.Join(", ", fieldAndValue)} WHERE {PKColumn} = {currType.GetProperty(PKColumn).GetValue(instance, null)};";
                        new MySqlCommand(sql, conn).ExecuteNonQuery();
                    }
                    else
                    {
                        string sql = $"INSERT INTO {currType.Name}({string.Join(", ", orderedFieldsNamesToSave)}) VALUES (\"{string.Join("\", \"", orderedFieldValuesToSave)}\");\nSELECT max({PKColumn}) FROM {currType.Name};";
                        MySqlDataReader rdr = new MySqlCommand(sql, conn).ExecuteReader();

                        // Update the personID of the now saved person.
                        rdr.Read();
                        PKValue = rdr[0].ToString();
                        try
                        {
                            currType.GetProperty(PKColumn).SetValue(instance, rdr[0]);
                        }
                        catch
                        {
                        }
                        rdr.Close();
                    }


                    foreach (PropertyInfo field in currentProperties)
                        savedFields.Add(field.Name);

                    return (PKValue, savedFields);
                }

                try
                {
                    conn.Open();

                    new MySqlCommand("START TRANSACTION;", conn).ExecuteNonQuery();

                    saveCurrentTable(instance.GetType());
                    instance.isSaved = true;

                    new MySqlCommand("COMMIT;", conn).ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    new MySqlCommand("ROLLBACK;", conn).ExecuteNonQuery();
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Retrieves the DataModels that satisfy the provided conditions.
        /// </summary>
        /// <typeparam name="T">Type of DataModel to retrieve instances of</typeparam>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static List<T> Filter<T>(Dictionary<string, object> conditions = null) where T : DataModel, new()
        {
            List<T> returnList = new List<T>();
            using (MySqlConnection conn = new MySqlConnection(DBManager.ConnectionString))
            {
                conn.Open();
                Type instanceType = typeof(T);
                T instance = DataController.CreateInstance<T>();

                HashSet<string> retrievedProperties = new HashSet<string>();

                string whereString = "";
                try
                {
                    IEnumerable<string> conditionString = conditions.Keys.Zip(conditions.Values, (a, b) => (a + " = " + b));
                    whereString = "WHERE" + string.Join(" AND ", conditionString);
                } 
                catch {}

                List<string> joinStrings = new List<string>();

                void joinCurrentTable(Type currType)
                {
                    if (_invalidBasetypes.Contains(currType.BaseType))
                        return;

                    // TODO Kevin: Maybe find a more foolproof way to get the current table primary key.
                    string idSuffix = (currType == instanceType ? "Id" : "");
                    joinStrings.Add($"INNER JOIN {currType.BaseType.Name} ON {currType.BaseType.Name}.{currType.BaseType.Name}{idSuffix} = {currType.Name}.{currType.BaseType.Name}");
                    joinCurrentTable(currType.BaseType);
                }

                joinCurrentTable(instanceType);

                new MySqlCommand($"USE {DatabaseManager.databaseName}", conn).ExecuteNonQuery();
                string sql = $"SELECT * FROM {instanceType.Name} {string.Join(" ", joinStrings)} {whereString}";
                MySqlDataReader rdr = new MySqlCommand(sql, conn).ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr.GetString(1));
                }
                rdr.Close();
            }

            return returnList;
        }

        /// <summary>
        /// Gets an instance of the specified type of DataModel, according to the specified conditions.
        /// </summary>
        /// <typeparam name="T">The type of DataModel to retrieve an instance of.</typeparam>
        /// <returns>The instance which satisfies the conditions.</returns>
        public static T Get<T>() where T : DataModel
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified DataModel.
        /// </summary>
        /// <param name="instance">DataModel to delete.</param>
        public static void Delete(DataModel instance)
        {
            throw new NotImplementedException();
        }

        public static T CreateInstance<T>() where T : DataModel
        {
            return Activator.CreateInstance<T>();
        }
    }

    public abstract class PersonController : DataController
    {

        public static T Getperson<T>(int id) where T : Person
        {
            throw new NotImplementedException();
        }

        public static T CreateInstance<T>(string firstName, string lastName) where T : Person
        {
            T person = Activator.CreateInstance<T>();
            person.FirstName = firstName;
            person.LastName = lastName;
            return person;
        }

        /// <summary> Removes the provided person from the specified teams. </summary>
        /// <param name="person"> Person which should be removed from the specified teams. </param>
        /// <param name="teams"> Enumerable of teams from which the person should be removed when present. </param>
        public static void RemoveFromTeams(Person person, IEnumerable<CampusTeamCollection> teams)
        {
            // Ensure that the specified teams intersect with the teams of the person.
            HashSet<CampusTeamCollection> inSet = new HashSet<CampusTeamCollection>(teams);
            inSet.IntersectWith(GetTeams(person));

            foreach (CampusTeamCollection team in inSet)
                team.Remove(person);
        }

        /// <summary> Removes the specified person from all teams in which it appears. </summary>
        /// <param name="person"> Person which should be removed from all its teams. </param>
        public static void RemoveFromTeams(Person person)
        {
            foreach (CampusTeamCollection team in GetTeams(person))
                team.Remove(person);
        }

        /// <summary> Removes the provided person from the specified team. </summary>
        /// <param name="person"> Person which should be removed. </param>
        /// <param name="team"> Team from which the person should be removed. </param>
        public static void RemoveFromTeams(Person person, CampusTeamCollection team)
        {
            team.Remove(person);
        }

        /// <summary> Adds the provided person to the specified team. </summary>
        /// <param name="person"> Person which should be added. </param>
        /// <param name="team"> Team to which the person should be added. </param>
        public static void AddToTeams(Person person, CampusTeamCollection team)
        {
            team.Add(person);
        }

        /// <summary> Adds the provided person to the specified teams. </summary>
        /// <param name="person"> Person which should be added. </param>
        /// <param name="team"> Teams to which the person should be added. </param>
        public static void AddToTeams(Person person, IEnumerable<CampusTeamCollection> teams)
        {
            foreach (CampusTeamCollection team in teams)
                team.Add(person);
        }

        /// <returns> A HashSet of the CampusTeams the provided person is included in. </returns>
        public static HashSet<CampusTeamCollection> GetTeams(Person person)  // GetTeams is convenient, but probably increases coupling.
        {
            HashSet<CampusTeamCollection> returnTeams = new HashSet<CampusTeamCollection>();
            foreach (CampusTeamCollection team in Collections.CampusTeams)
                if (team.Contains(person))
                    returnTeams.Add(team);
            return returnTeams;
        }

        /// <summary>
        /// Finds related people of the specified type.
        /// Example: Find students for the specified teacher.
        /// </summary>
        /// <typeparam name="T">Type of person which should be found and returned in related classrooms.</typeparam>
        /// <param name="person">Person for which related people should be found.</param>
        /// <returns>Hashset of related people of the specified type.</returns>
        public static HashSet<T> GetRelatedTeam<T>(Person person) where T : Person
        {
            HashSet<T> returnSet = new HashSet<T>();
            foreach (CampusTeamCollection team in PersonController.GetTeams(person))
                if (team.Contains(person))
                    foreach (T relatedPerson in team.membersOfType<T>())
                        returnSet.Add(relatedPerson);
            return returnSet;
        }


    }
}
