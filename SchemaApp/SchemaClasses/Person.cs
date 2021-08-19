using System;
using System.Collections.Generic;

namespace SchemaClasses
{

    public abstract class Person : ILogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }

        /// <returns> A HashSet of the CampusTeams this person is included in. </returns>
        public HashSet<CampusTeam> GetTeams()  // GetTeams is convenient, but probably increases coupling.
        {
            HashSet <CampusTeam> returnTeams = new HashSet<CampusTeam>();
            foreach (CampusTeam team in Collections.CampusTeams)
                if (team.Contains(this))
                    returnTeams.Add(team);
            return returnTeams;
        }

        public Person()
        {
            Collections.People.Add(this);
        }
        public Person(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
            Collections.People.Add(this);
        }

        ~Person()
        {
            Collections.People.Remove(this);
        }

        /// <summary>
        /// Useful when the user itself needs to be paired with matching login credentials.
        /// </summary>
        /// <returns>A KeyValuePair which may be added 'directly' to the users dictionary.</returns>
        public KeyValuePair<Login, Person> Login() => new KeyValuePair<Login, Person>(new Login(this), this);

        public override string ToString() => $"[{this.UserName}, {this.GetType().Name}]";
    }

    public class Student : Person
    {
        public Student(string username, string password) : base(username, password)
        {
        }
    }

    public class Teacher : Person
    {
        public Teacher(string username, string password) : base(username, password)
        {
        }
    }
}
