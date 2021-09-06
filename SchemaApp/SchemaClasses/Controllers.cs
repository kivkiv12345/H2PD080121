﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static SchemaClasses.Exceptions;

namespace SchemaClasses
{
    public static class PersonController
    {

        public static T Getperson<T>(int id) where T : Person
        {
            throw new NotImplementedException();
        }

        public static T CreatePerson<T>(string firstName, string password) where T : Person
        {
            T person = Activator.CreateInstance<T>();
            person.FirstName = firstName;
            person.Password = password;
            return person;
        }

        /// <summary> Removes the provided person from the specified teams. </summary>
        /// <param name="person"> Person which should be removed from the specified teams. </param>
        /// <param name="teams"> Enumerable of teams from which the person should be removed when present. </param>
        public static void RemoveFromTeams(Person person, IEnumerable<CampusTeam> teams)
        {
            // Ensure that the specified teams intersect with the teams of the person.
            HashSet<CampusTeam> inSet = new HashSet<CampusTeam>(teams);
            inSet.IntersectWith(GetTeams(person));

            foreach (CampusTeam team in inSet)
                team.Remove(person);
        }

        /// <summary> Removes the specified person from all teams in which it appears. </summary>
        /// <param name="person"> Person which should be removed from all its teams. </param>
        public static void RemoveFromTeams(Person person)
        {
            foreach (CampusTeam team in GetTeams(person))
                team.Remove(person);
        }

        /// <summary> Removes the provided person from the specified team. </summary>
        /// <param name="person"> Person which should be removed. </param>
        /// <param name="team"> Team from which the person should be removed. </param>
        public static void RemoveFromTeams(Person person, CampusTeam team)
        {
            team.Remove(person);
        }

        /// <summary> Adds the provided person to the specified team. </summary>
        /// <param name="person"> Person which should be added. </param>
        /// <param name="team"> Team to which the person should be added. </param>
        public static void AddToTeams(Person person, CampusTeam team)
        {
            team.Add(person);
        }

        /// <summary> Adds the provided person to the specified teams. </summary>
        /// <param name="person"> Person which should be added. </param>
        /// <param name="team"> Teams to which the person should be added. </param>
        public static void AddToTeams(Person person, IEnumerable<CampusTeam> teams)
        {
            foreach (CampusTeam team in teams)
                team.Add(person);
        }
        /// <summary>
        /// Useful when the user itself needs to be paired with matching login credentials.
        /// </summary>
        /// <returns>A KeyValuePair which may be added 'directly' to the users dictionary.</returns>
        public static KeyValuePair<Login, Person> Login(Person person) => new KeyValuePair<Login, Person>(new Login(person), person);

        /// <returns> A HashSet of the CampusTeams the provided person is included in. </returns>
        public static HashSet<CampusTeam> GetTeams(Person person)  // GetTeams is convenient, but probably increases coupling.
        {
            HashSet<CampusTeam> returnTeams = new HashSet<CampusTeam>();
            foreach (CampusTeam team in Collections.CampusTeams)
                if (team.Contains(person))
                    returnTeams.Add(team);
            return returnTeams;
        }

        /// <summary>
        /// Finds related people of the specified type.
        /// Example: Find students for the specified teacher.
        /// </summary>
        /// <typeparam name="T">Type of person which should be found and returned in related classrooms.</typeparam>
        /// <param name="person">Person for which related people should be found.</param>
        /// <returns>Hashset of related people of the specified type.</returns>
        public static HashSet<T> GetRelatedTeam<T>(Person person) where T : Person
        {
            HashSet<T> returnSet = new HashSet<T>();
            foreach (CampusTeam team in PersonController.GetTeams(person))
                if (team.Contains(person))
                    foreach (T relatedPerson in team.membersOfType<T>())
                        returnSet.Add(relatedPerson);
            return returnSet;
        }

        public static void Save(Person person)
        {
            try
            {
                person.validate();
            } catch(ValidationError e)
            {
                // TODO Kevin: Perhaps handle user feedback here.
                throw e;
            }

            HashSet<PropertyInfo> personPropSet = new HashSet<PropertyInfo>(typeof(Person).GetProperties());
            Console.WriteLine("Stuff");
            HashSet<PropertyInfo> tPropSet = new HashSet<PropertyInfo>(person.GetType().GetProperties());
            tPropSet.ExceptWith(personPropSet);
            foreach (PropertyInfo prop in tPropSet)
                if (personPropSet.Contains(prop))
                    Console.WriteLine("inside");
            Console.WriteLine("lol");
        }
    }
}
