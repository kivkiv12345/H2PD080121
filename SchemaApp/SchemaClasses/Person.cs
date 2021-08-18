using System;

namespace SchemaClasses
{
    public enum Sex
    {
        MALE,
        FEMALE,
    }

    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public Sex Sex { get; set; }
    }

    public class Student : Person
    {

    }

    public class Teacher : Person
    {

    }
}
