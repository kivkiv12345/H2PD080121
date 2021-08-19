using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    public class CampusTeam : HashSet<Person>
    {
        public string Name { get; set; }
        public List<T> membersOfType<T>() where T : class
        {
            List<T> returnMembers = new List<T>();
            foreach (Person person in this)
                if (person.GetType() == typeof(T))
                    returnMembers.Add((T)(object)person);
            return returnMembers;
        }

        // We don't actually care about the signatures for our constructors,
        // We only care about ensuring that 'this' is added to the CampusTeams collection.
        // But since C# is a pretty limiting language,
        // we can't generically duplicate parent constructor signatures (nor do we feature a post-constructor hook),
        // we must break DRY principles and write them here
        public CampusTeam() : base()
        {
            Collections.CampusTeams.Add(this);
        }

        public CampusTeam(int capacity) : base(capacity)
        {
            Collections.CampusTeams.Add(this);
        }

        ~CampusTeam()
        {
            Collections.CampusTeams.Remove(this);
        }
    }
}
