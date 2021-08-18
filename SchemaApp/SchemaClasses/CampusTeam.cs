using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    class CampusTeam : List<Person>
    {
        public string Name { get; set; }
        public List<T> membersOfType<T>() where T : class
        {
            List<T> checkedMembers = new List<T>();

            foreach (Person person in this)
            {
                if (person.GetType() == typeof(T))
                    checkedMembers.Add((T)(object)person);
            }
            return checkedMembers;
        }
    }
}
