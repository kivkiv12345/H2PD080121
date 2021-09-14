using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    class DatabaseManager
    {
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
