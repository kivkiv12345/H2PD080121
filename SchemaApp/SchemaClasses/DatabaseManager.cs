using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    /// <summary>
    /// This class represents the base class for models.
    /// It is specifically designed for serializing classes for databases.
    /// Properties with names suffixed by "ID" are taken to be primary keys.
    /// </summary>
    public abstract class DBModel
    {
        public Validator validate;

        public delegate void Validator();
    }

    public class DatabaseManager
    {
        public static readonly string ConnectionString = "server=localhost;user=root;database=schema_H2;port=3306;password=Test1234!";

        /// <summary>
        /// Creates database tables for the specified models.
        /// </summary>
        public void InitDatabase(IEnumerable<Type> models) {
            throw new NotImplementedException();
        }

        public void test()
        {
            InitDatabase(new Type[] { 
                typeof(Person),
                typeof(Student),
                typeof(Teacher),
            });
        }
    }
}
