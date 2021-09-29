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
            Console.WriteLine("Please enter the MySQL password");
            PersonController.DBManager = new DatabaseManager(Console.ReadLine());

            // TODO Kevin: Testing stuff.
            Student person = PersonController.CreateInstance<Student>("Kiv", "Test1234!");
            person.LastName = "lol";
            CampusTeam team = DBController.CreateInstance<CampusTeam>();
            team.teamName = "hhhhhhhhh70";
            person.Team = team;
            PersonController.Save(person);
            Environment.Exit(0);

            CampusTeam teamm = DBController.CreateInstance<CampusTeam>();
            teamm.teamName = "HHHHHHHHHH1";
            DBController.Save(team);
            Environment.Exit(0);
            
            
        }
    }
}
