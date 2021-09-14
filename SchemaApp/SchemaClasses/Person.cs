using System;
using System.Collections.Generic;
using static SchemaClasses.Exceptions;

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

        private void _ValidateName()
        {
            HashSet<object> _invalid_types = new HashSet<object>{ null, "", string.Empty};
            if (_invalid_types.Contains(this.FirstName) || _invalid_types.Contains(this.LastName))
                throw new ValidationError("Cannot save person without a name.");
        }

        public override string ToString() => $"[{this.UserName}, {this.GetType().Name}]";

        public Validator validate;

        public delegate void Validator();

        public Person()
        {
            // It seems to be impossible to define instance validators outside of a class.
            // So that's why we have to define the validators inside the class itself, and involve the constructor.
            this.validate = new Validator(this._ValidateName);
        }

    }

    public sealed class Student : Person
    {
        public int Grade { get; set; }  // TODO Kevin: Testing

        public void _ValidateStudent()
        {

        }

        public Student() : base()
        {
            this.validate += new Validator(this._ValidateStudent);
        }
    }

    public sealed class Teacher : Person
    {
    }
}
