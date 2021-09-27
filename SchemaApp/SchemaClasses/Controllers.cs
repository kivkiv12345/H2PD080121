using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SchemaClasses
{
    public abstract class DBController
    {
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

        private static IEnumerable<(string, object)> getPrimaryKeys(DBModel instance)
        {
            return from field in instance.GetType().GetProperties() where field.Name.ToLower().EndsWith("id") select (field.Name, field.GetValue(instance, null));
        }

        private enum SaveModes
        {
            INSERT,
            UPDATE,
        }

        public static void Save(DBModel instance, bool validate = true)
        {
            if (validate)
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
            }

            string _convertSaveString(PropertyInfo prop)
            {
                var value = prop.GetValue(instance, null);
                try
                {
                    return saveConversions[prop.PropertyType](value);
                }
                catch (KeyNotFoundException)
                {
                    return (value ?? "null").ToString();
                }
            }

            Type instanceType = instance.GetType();

            /// <summary>
            /// Saves a specific class to a specific table. Handles subclasses.
            /// </summary>
            /// <param name="currTupe">A subclass of DBModel, of which properties should be saved.</param>
            /// <returns>The properties that were saved.</returns>
            (string?, HashSet<string>) saveCurrentTable(Type currType, SaveModes mode, MySqlConnection conn)  // Method inspired by: https://stackoverflow.com/questions/8868119/find-all-parent-types-both-base-classes-and-interfaces
            {
                // TODO Kevin: We might not want the connection to be passed as a parameter.

                // We just attempted to save a nonexistent base class (most likely because we just walked to the top the inheritance hierarchy),
                // hence no fields were saved. We therefore create and return an empty hashset, indicating to the previous recursion level,
                // that all its found fields should be saved under its model.
                if (currType == null)
                    return (null, new HashSet<string>());

                // These are the fields that have already been handled. We need them to know which current fields to exclude.
                (string PKValue, HashSet<string> savedFields) = saveCurrentTable(currType.BaseType, mode, conn);

                // These are the field names, followed by their values, that should be saved at this recursion level.
                List<string> orderedFieldsNamesToSave = (from field in currType.GetProperties() where !savedFields.Contains(field.Name) && field.GetValue(instance, null) != null select field.Name).ToList();
                List<string> orderedFieldValuesToSave = (from fieldName in orderedFieldsNamesToSave select _convertSaveString(currType.GetProperty(fieldName))).ToList();

                object[] _invalidBasetypes = new object[] { typeof(DBModel), null };

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
                if (currType.BaseType == typeof(DBModel))
                {
                    IEnumerable<string> PKColumns = from field in currType.GetProperties() where field.Name.ToLower().EndsWith("id") select field.Name;
                    if (PKColumns.Count() != 1)
                        throw new Exception("Database models using inheritance must define exactly 1 primary key column.");
                    PKColumn = PKColumns.First();
                } else
                {
                    string baseName = currType.BaseType.Name;
                    PKColumn = baseName[0].ToString().ToLower() + baseName[1..];  // Converting from pascal case to camel case.
                }

                if (mode == SaveModes.INSERT)
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
                } else if (mode == SaveModes.UPDATE)
                {
                    IEnumerable<string> fieldAndValue = orderedFieldsNamesToSave.Zip(orderedFieldValuesToSave, (a, b) => a + " = " + b);
                    string sql = $"UPDATE {currType.Name} SET {string.Join(", ", fieldAndValue)} WHERE {PKColumn} = {currType.GetProperty(PKColumn).GetValue(instance, null)};";
                    new MySqlCommand(sql, conn).ExecuteNonQuery();
                } else
                {
                    throw new Exception("Unsupported save mode");
                }
                

                foreach (string fieldName in orderedFieldsNamesToSave)
                    savedFields.Add(fieldName);

                return (PKValue, savedFields);
            }

            using (MySqlConnection conn = new MySqlConnection(DatabaseManager.ConnectionString))
            {
                try
                {
                    conn.Open();

                    new MySqlCommand("START TRANSACTION;", conn).ExecuteNonQuery();

                    SaveModes mode;
                    // Saving a new person, uses INSERT 
                    if ((from tuple in getPrimaryKeys(instance) where tuple.Item2 == null select tuple).Count() != 0)
                    {
                        mode = SaveModes.INSERT;
                    }
                    else  // Saving an existing person, uses UPDATE
                    {
                        mode = SaveModes.UPDATE;
                    }

                    saveCurrentTable(instance.GetType(), mode, conn);

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

        public static T CreateInstance<T>() where T : DBModel
        {
            return Activator.CreateInstance<T>();
        }
    }

    public abstract class PersonController : DBController
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
        public static void RemoveFromTeams(Person person, IEnumerable<CampusTeam> teams)
        {
            // Ensure that the specified teams intersect with the teams of the person.
            HashSet<CampusTeam> inSet = new HashSet<CampusTeam>(teams);
            inSet.IntersectWith(GetTeams(person));

            foreach (CampusTeam team in inSet)
                team.Remove(person);
        }

        /// <summary> Removes the specified person from all teams in which it appears. </summary>
        /// <param name="person"> Person which should be removed from all its teams. </param>
        public static void RemoveFromTeams(Person person)
        {
            foreach (CampusTeam team in GetTeams(person))
                team.Remove(person);
        }

        /// <summary> Removes the provided person from the specified team. </summary>
        /// <param name="person"> Person which should be removed. </param>
        /// <param name="team"> Team from which the person should be removed. </param>
        public static void RemoveFromTeams(Person person, CampusTeam team)
        {
            team.Remove(person);
        }

        /// <summary> Adds the provided person to the specified team. </summary>
        /// <param name="person"> Person which should be added. </param>
        /// <param name="team"> Team to which the person should be added. </param>
        public static void AddToTeams(Person person, CampusTeam team)
        {
            team.Add(person);
        }

        /// <summary> Adds the provided person to the specified teams. </summary>
        /// <param name="person"> Person which should be added. </param>
        /// <param name="team"> Teams to which the person should be added. </param>
        public static void AddToTeams(Person person, IEnumerable<CampusTeam> teams)
        {
            foreach (CampusTeam team in teams)
                team.Add(person);
        }

        /// <returns> A HashSet of the CampusTeams the provided person is included in. </returns>
        public static HashSet<CampusTeam> GetTeams(Person person)  // GetTeams is convenient, but probably increases coupling.
        {
            HashSet<CampusTeam> returnTeams = new HashSet<CampusTeam>();
            foreach (CampusTeam team in Collections.CampusTeams)
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
            foreach (CampusTeam team in PersonController.GetTeams(person))
                if (team.Contains(person))
                    foreach (T relatedPerson in team.membersOfType<T>())
                        returnSet.Add(relatedPerson);
            return returnSet;
        }

        
    }
}
