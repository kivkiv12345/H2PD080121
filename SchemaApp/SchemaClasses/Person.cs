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

        public override string ToString() => $"[{this.UserName}, {this.GetType().Name}]";
    }

    public class Student : Person
    {
    }

    public class Teacher : Person
    {
    }
}
