using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{

    public enum Sex
    {
        MALE,
        FEMALE,
    }

    public interface ILogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// The Login class is used to accept user input, and find the corresponding campus member in the users dictionary.
    /// </summary>
    public class Login
    {
        string username, password;  // The name and password strings are what will be used to find the corresponding user in the dictionary.

        public Login(ILogin member)
        {
            this.username = member.UserName;
            this.password = member.Password;
        }

        public Login(string name, string password)
        {
            this.username = name;
            this.password = password;
        }

        public override int GetHashCode() => (username + password).GetHashCode();

        public override bool Equals(object obj) => Equals(obj as Login);
        public bool Equals(Login obj) => obj != null && obj.GetHashCode() == this.GetHashCode();

        public override string ToString() => $"Login for {username}";
    }
}
