using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    /// <summary>
    /// Static class which holds different collections of classes from the object model.
    /// </summary>
    static class Collections
    {
        public static HashSet<CampusTeam> CampusTeams = new HashSet<CampusTeam>();

        public static HashSet<Person> People = new HashSet<Person>();

        public static HashSet<T> PeopleOfType<T>()
        {
            HashSet<T> returnMembers = new HashSet<T>();
            foreach (Person person in People)
                if (person.GetType() == typeof(T))
                    returnMembers.Add((T)(object)person);
            return returnMembers;
        }
    }
}
