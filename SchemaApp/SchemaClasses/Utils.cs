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

    public enum Grades
    {
        _12 = 12,
        _10 = 10,
        _7 = 7,
        _4 = 4,
        _02 = 2,
        _00 = 0,
        _03 = -3,
    }



    /// <summary>
    /// This interface is almost entirely pointless, but nevertheless here as an example. 
    /// A better example would've been possible if C# allowed interfaces to define static members.
    /// </summary>
    public interface ICrudOps<T>
    {
        public static List<T> Filter(Dictionary<string, object> conditions = null)
        {
            throw new CSharpIsStupidException("Microsoft (in all their glorious wisdom) has decreed that static members in " +
                $@"interfaces must provide a default implementation.{Environment.NewLine}So here it is i guess... ¯\_(◡_◡)_/¯");
        }

        public static T Get(Dictionary<string, object> conditions = null)
        {
            throw new CSharpIsStupidException("Microsoft (in all their glorious wisdom) has decreed that static members in " +
                $@"interfaces must provide a default implementation.{Environment.NewLine}So here it is i guess... ¯\_(◡_◡)_/¯");
        }

        public static void Delete(DataModel instance)
        {
            throw new CSharpIsStupidException("Microsoft (in all their glorious wisdom) has decreed that static members in " +
                $@"interfaces must provide a default implementation.{Environment.NewLine}So here it is i guess... ¯\_(◡_◡)_/¯");
        }

        public static T CreateInstance()
        {
            throw new CSharpIsStupidException("Microsoft (in all their glorious wisdom) has decreed that static members in " +
                $@"interfaces must provide a default implementation.{Environment.NewLine}So here it is i guess... ¯\_(◡_◡)_/¯");
        }

        public static void Save(DataModel instance)
        {
            throw new CSharpIsStupidException("Microsoft (in all their glorious wisdom) has decreed that static members in " +
                $@"interfaces must provide a default implementation.{Environment.NewLine}So here it is i guess... ¯\_(◡_◡)_/¯");
        }
    }

    /// <summary>
    /// This class is handy for creating CRUD operations for DataModel that are likely to be encountered at runtime.
    /// It is also incredibly stupid, meaning there has to be a better way.
    /// </summary>
    /// <typeparam name="T">The DataModel to use in CRUD operations.</typeparam>
    public class RuntimeTypeDataController<T> : ICrudOps<T> where T : DataModel, new()
    {
        public List<T> Filter(Dictionary<string, object> conditions = null)
        {
            return DataController.Filter<T>(conditions);
        }

        public T Get(Dictionary<string, object> conditions = null)
        {
            return DataController.Get<T>(conditions);
        }

        public void Delete(DataModel instance)
        {
            DataController.Delete(instance);
        }

        public T CreateInstance()
        {
            return DataController.CreateInstance<T>();
        }

        public void Save(DataModel instance)
        {
            DataController.Save(instance);
        }
    }

    public static class ThisHasToBeHereCauseCSharpIsStupid
    {
        /// <summary>
        /// This dictionary holds CRUD operations that may be retrieved at runtime.
        /// The RuntimeTypeDataController values are stored as dynamic because C# is stupid,
        /// and requires the specification of value generic types.
        /// </summary>
        public static readonly Dictionary<Type, dynamic> RuntimeCRUDDict = new Dictionary<Type, dynamic>()
        {
            { typeof(Student), new RuntimeTypeDataController<Student>() },
            { typeof(Teacher), new RuntimeTypeDataController<Teacher>() },
            { typeof(Subject), new RuntimeTypeDataController<Subject>() },
            { typeof(CampusTeam), new RuntimeTypeDataController<CampusTeam>() },
        };
    }
}
