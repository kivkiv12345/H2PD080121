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
            Person person = PersonController.CreateInstance<Student>("Kiv", "Test1234!");
            person.LastName = "lol";
            PersonController.Save(person);
        }
    }
}
