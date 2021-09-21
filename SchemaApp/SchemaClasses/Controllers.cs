using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static SchemaClasses.Exceptions;

namespace SchemaClasses
{
    public static class PersonController
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

        public static void Save(Person person, bool validate = true)
        {
            if (validate)
            {
                try
                {
                    person.validate();
                }
                catch (ValidationError e)
                {
                    // TODO Kevin: Perhaps handle user feedback here.
                    throw e;
                }
            }

            // Get the properties of the Person class.
            HashSet<PropertyInfo> personPropSet = new HashSet<PropertyInfo>(typeof(Person).GetProperties());

            // Get the names of the properties of the Person class.
            HashSet<string> personPropSetNames = new HashSet<string>(from prop in typeof(Person).GetProperties() select prop.Name);

            // TODO Kevin:

            // Get all the properties of the subclass, which aren't defined by Person.
            HashSet<PropertyInfo> tPropSet = new HashSet<PropertyInfo>(
                from prop in person.GetType().GetProperties() where !personPropSetNames.Contains(prop.Name) select prop);

            string _convertSaveString(PropertyInfo prop)
            {
                var value = prop.GetValue(person, null);
                try
                {
                    return saveConversions[prop.PropertyType](value);
                }
                catch (KeyNotFoundException)
                {
                    return (value ?? "null").ToString();
                }
            }

            Type personType = person.GetType();

            using (MySqlConnection conn = new MySqlConnection(DatabaseManager.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Saving a new person, uses INSERT 
                    if (person.personID == null)
                    {
                        // As hashsets are not ordered, we create these arrays, such that the property names and values are placed on the same indexes.
                        var personPropNameArray = from propName in personPropSetNames where personType.GetProperty(propName).GetValue(person, null) != null select propName;
                        var personPropValueArray = from propName in personPropNameArray select _convertSaveString(personType.GetProperty(propName));

                        var tPropNameArray = from prop in tPropSet where personType.GetProperty(prop.Name).GetValue(person, null) != null select prop.Name;
                        var tPropValueArray = from propName in tPropNameArray select _convertSaveString(personType.GetProperty(propName));

                        string sql = $"START TRANSACTION; USE schema_h2;\nINSERT INTO Person({string.Join(", ", personPropNameArray)}) VALUES (\"{string.Join("\", \"", personPropValueArray)}\");\nSELECT max(personID) FROM Person;\nCOMMIT;";
                        MySqlDataReader rdr = new MySqlCommand(sql, conn).ExecuteReader();

                        // Update the personID of the now saved person.
                        rdr.Read();
                        person.personID = Convert.ToUInt64(rdr[0]);
                        rdr.Close();

                        // TODO Kevin: SQL is broken when no other properties are present.
                        sql = $"INSERT INTO {person.GetType().Name}(person, {string.Join(", ", tPropNameArray)}) VALUES ({person.personID}, \"{string.Join("\", \"", tPropValueArray)}\");";
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
}
