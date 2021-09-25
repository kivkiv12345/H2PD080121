using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaClasses
{
    /// <summary>
    /// Thrown when a validation check fails.
    /// </summary>
    public class ValidationError : Exception
    {
        public ValidationError()
        {
        }

        public ValidationError(string message)
            : base(message)
        {
        }

        public ValidationError(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Used in situations where C# denies or lacks obvious functionality and or features.
    /// In such situations, this exception may be thrown following sanity checks during runtime.
    /// </summary>
    public class CSharpIsStupidException : Exception
    {
        public CSharpIsStupidException()
        {
        }

        public CSharpIsStupidException(string message)
            : base(message)
        {
        }

        public CSharpIsStupidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
