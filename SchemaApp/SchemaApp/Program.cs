using SchemaClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;
using static SchemaClasses.DatabaseManager;

namespace SchemaApp
{
    class Program
    {
        public static Person CurrentUser = null;

        public static IDictionary<Login, Person> users = new List<KeyValuePair<Login, Person>>
        {
            // Initial members are added in this list.
            PersonController.Login(PersonController.CreatePerson<Teacher>("Egon", "IsNice")),
            PersonController.Login(PersonController.CreatePerson<Student>("Kiv", "Test1234!")),
            PersonController.Login(PersonController.CreatePerson<Student>("Dig", "qwerty")),
        }.ToDictionary(x => x.Key, x => x.Value);

        static void Main(string[] args)
        {
            /*Console.WriteLine("Please enter the MySql root password:");

            using (MySqlConnection conn = new MySqlConnection(DatabaseManager.ConnectionString))
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = "SELECT * FROM Person";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                conn.Close();
                Console.WriteLine("Done.");
            }

            Environment.Exit(0);*/

            // TODO Kevin: Testing stuff.
            Person person = PersonController.CreatePerson<Student>("Kiv", "Test1234!");
            person.LastName = "lol";
            PersonController.Save(person);

            Environment.Exit(0);

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
            }
        }
    }
}
