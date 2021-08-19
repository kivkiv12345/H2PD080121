using SchemaClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SchemaApp
{
    class Program
    {
        public static Person CurrentUser = null;

        public static IDictionary<Login, Person> users = new List<KeyValuePair<Login, Person>>
        {
            // Initial members are added in this list.
            new Teacher("Egon", "IsNice").Login(),
            new Student("Kiv", "Test1234!").Login(),
            new Student("Dig", "qwerty").Login(),
        }.ToDictionary(x => x.Key, x => x.Value);

        static void Main(string[] args)
        {
            while (true)
            {

                bool again = true;
                while (again)
                {
                    again = false;

                    Console.WriteLine("Please enter your username: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Please enter your password: ");
                    string password = Console.ReadLine();
                    Console.Clear();
                    try
                    {
                        CurrentUser = users[new Login(name, password)];
                        Console.WriteLine($"Now logging in as {CurrentUser}.");
                    }
                    catch (KeyNotFoundException)
                    {
                        again = true;
                        Console.WriteLine("Incorrect username or password. Please try again.");
                    }
                }
                // TODO Kevin: Maybe add a loading animation here.
                Thread.Sleep(1500);

                while (CurrentUser != null)
                {
                    Console.Clear();

                    Console.WriteLine($"Welcome {CurrentUser.UserName}");

                    Console.WriteLine("These are your options: ");

                    //CurrentUser.PrintActions();

                    if (CurrentUser != null)
                        Console.ReadLine();
                    else
                        Console.Clear();
                }
            }
        }
    }
}
