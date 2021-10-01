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
    public abstract class DataModel
    {
        public bool isSaved = false;

        public Validator validate;

        public delegate void Validator();
    }

    public class DatabaseManager
    {
        public string password = null;
        public string username = "root";
        public string host = "localhost";
        public int port = 3306;

        public string ConnectionString 
        { 
            get {
                return $"server={this.host};user={this.username};database=schema_H2;port={this.port};password={this.password}"; 
            } 
        }

        public const string databaseName = "schema_H2";

        public DatabaseManager(string password)
        {
            this.password = password;
        }

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
