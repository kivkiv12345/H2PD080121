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

        /// <summary>
        /// Useful when the user itself needs to be paired with matching login credentials.
        /// </summary>
        /// <returns>A KeyValuePair which may be added 'directly' to the users dictionary.</returns>
        public KeyValuePair<Login, Person> Login() => new KeyValuePair<Login, Person>(new Login(this), this);

        public override string ToString() => $"[{this.UserName}, {this.GetType().Name}]";
    }

    public class Student : Person
    {
    }

    public class Teacher : Person
    {
    }
}
