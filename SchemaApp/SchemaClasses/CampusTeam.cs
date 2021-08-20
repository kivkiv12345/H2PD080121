using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    public class CampusTeam : HashSet<Person>
    {
        public string Name { get; set; }
        public List<T> membersOfType<T>() where T : Person
        {
            List<T> returnMembers = new List<T>();
            foreach (Person person in this)
                if (person.GetType() == typeof(T))
                    returnMembers.Add((T)person);
            return returnMembers;
        }
    }
}
