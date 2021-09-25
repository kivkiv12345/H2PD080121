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
            }

            // TODO Kevin: We must recursively walk upwards in the class hierarchy to get and save the attributes of each class individually.

            // Get the properties of the Person class.
            HashSet<PropertyInfo> personPropSet = new HashSet<PropertyInfo>(typeof(Person).GetProperties());

            // Get the names of the properties of the Person class.
            HashSet<string> personPropSetNames = new HashSet<string>(from prop in typeof(Person).GetProperties() select prop.Name);

            // TODO Kevin:

            // Get all the properties of the subclass, which aren't defined by Person.
            HashSet<PropertyInfo> tPropSet = new HashSet<PropertyInfo>(
                from prop in instance.GetType().GetProperties() where !personPropSetNames.Contains(prop.Name) select prop);

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

            /// <summary>
            /// Saves a specific class to a specific table. Handles subclasses.
            /// </summary>
            /// <param name="currTupe">A subclass of DBModel, of which properties should be saved.</param>
            /// <returns>The properties that were saved.</returns>
            (string?, HashSet<string>) saveCurrentTable(Type currType, MySqlConnection conn)  // Method inspired by: https://stackoverflow.com/questions/8868119/find-all-parent-types-both-base-classes-and-interfaces
            {
                // TODO Kevin: We might not want the connection to be passed as a parameter.
                // TODO Kevin: Find a way to handle primary keys.

                // We just attempted to save a nonexistent base class (most likely because we just walked to the top the inheritance hierarchy),
                // hence no fields were saved. We therefore create and return an empty hashset, indicating to the previous recursion level,
                // that all its found fields should be saved under its model.
                if (currType == null)
                    return (null, new HashSet<string>());

                // These are the fields that have already been handled. We need them to know which current fields to exclude.
                (string PKValue, HashSet<string> savedFields) = saveCurrentTable(currType.BaseType, conn);

                // These are the field names, followed by their values, that should be saved at this recursion level.
                IEnumerable<string> orderedFieldsNamesToSave = from field in currType.GetProperties() where !savedFields.Contains(field.Name) select field.Name;
                IEnumerable<string> orderedFieldValuesToSave = from fieldName in orderedFieldsNamesToSave select _convertSaveString(currType.GetProperty(fieldName));

                // The current model doesn't define any unsaved fields, so we don't save anything for it.
                // TODO Kevin: This may not be the case for certain intermediate models where primary keys still need to be handled.
                if (orderedFieldsNamesToSave.Count() == 0)
                    return (PKValue, savedFields);

                // TODO Kevin use somehow: currType.BaseType.Name

                string sql = $"INSERT INTO {currType.Name}({string.Join(", ", orderedFieldsNamesToSave)}) VALUES (\"{string.Join("\", \"", orderedFieldValuesToSave)}\");";
                new MySqlCommand(sql, conn).ExecuteNonQuery();

                foreach (string fieldName in orderedFieldsNamesToSave)
                    savedFields.Add(fieldName);

                return (PKValue, savedFields);
            }

            Type personType = instance.GetType();

            using (MySqlConnection conn = new MySqlConnection(DatabaseManager.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Saving a new person, uses INSERT 
                    if ((from tuple in getPrimaryKeys(instance) where tuple.Item2 != null select tuple).Count() != 0)
                    {
                        // As hashsets are not ordered, we create these arrays, such that the property names and values are placed on the same indexes.
                        var personPropNameArray = from propName in personPropSetNames where personType.GetProperty(propName).GetValue(instance, null) != null select propName;
                        var personPropValueArray = from propName in personPropNameArray select _convertSaveString(personType.GetProperty(propName));

                        var tPropNameArray = from prop in tPropSet where personType.GetProperty(prop.Name).GetValue(instance, null) != null select prop.Name;
                        var tPropValueArray = from propName in tPropNameArray select _convertSaveString(personType.GetProperty(propName));

                        string sql = $"START TRANSACTION; USE schema_h2;\nINSERT INTO Person({string.Join(", ", personPropNameArray)}) VALUES (\"{string.Join("\", \"", personPropValueArray)}\");\nSELECT max(personID) FROM Person;\nCOMMIT;";
                        MySqlDataReader rdr = new MySqlCommand(sql, conn).ExecuteReader();

                        // Update the personID of the now saved person.
                        rdr.Read();
                        instance.personID = Convert.ToUInt64(rdr[0]);
                        rdr.Close();

                        // TODO Kevin: SQL is broken when no other properties are present.
                        sql = $"INSERT INTO {instance.GetType().Name}(person, {string.Join(", ", tPropNameArray)}) VALUES ({instance.personID}, \"{string.Join("\", \"", tPropValueArray)}\");";
                        new MySqlCommand(sql, conn).ExecuteReader();
                    }
                    else  // Saving an existing person, uses UPDATE
                    {

                    }


                }
                catch (Exception ex)
                {
                    new MySqlCommand("rollback;", conn).ExecuteReader();
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

    public abstract class PersonController : DBController
    {

        public static T Getperson<T>(int id) where T : Person
        {
            throw new NotImplementedException();
        }

        public static T CreatePerson<T>(string firstName, string lastName) where T : Person
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
