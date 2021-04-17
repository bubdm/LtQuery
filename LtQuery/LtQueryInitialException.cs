using System;

namespace LtQuery
{
    public class LtQueryInitialException : LtQueryException
    {
        public LtQueryInitialException(string message) : base(message) { }
        public LtQueryInitialException(string message, Exception innerException) : base(message, innerException) { }
    }
}
