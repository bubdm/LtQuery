using System;

namespace LtQuery
{
    public class LtQueryException : Exception
    {
        public LtQueryException(string message) : base(message) { }
        public LtQueryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
