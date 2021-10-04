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
            //Console.WriteLine("Please enter the MySQL password");
            //PersonController.DBManager = new DatabaseManager(Console.ReadLine());
            PersonController.DBManager = new DatabaseManager("Test1234!");

            List<Student> students = PersonController.Filter<Student>();
            Environment.Exit(0);

            // TODO Kevin: Testing stuff.
            Student person = PersonController.CreateInstance<Student>("Kiv", "lololman");
            CampusTeam team = DataController.CreateInstance<CampusTeam>();
            team.teamName = "hhhhhhhhh70";
            person.Team = team;
            PersonController.Save(person);
            Environment.Exit(0);

            CampusTeam teamm = DataController.CreateInstance<CampusTeam>();
            teamm.teamName = "HHHHHHHHHH1";
            DataController.Save(team);
            Environment.Exit(0);
            
            
        }
    }
}
