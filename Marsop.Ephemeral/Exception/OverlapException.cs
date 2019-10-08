using System;

namespace Marsop.Ephemeral
{
    public class OverlapException : ArgumentException
    {
        public OverlapException() { }

        public OverlapException(string message) : base(message) { }

        public OverlapException(string message, string paramName) : base(message, paramName) { }

        public OverlapException(string message, Exception innerException) : base(message, innerException) { }

        public OverlapException(string message, string paramName, Exception innerException) :
        base(message, paramName, innerException)
        { }
    }
}