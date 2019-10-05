using System;

namespace Marsop.Ephemeral
{
    public class InvalidDurationException : ArgumentException
    {
        public InvalidDurationException() { }

        public InvalidDurationException(string message) : base(message) { }

        public InvalidDurationException(string message, string paramName) : base(message, paramName) { }

        public InvalidDurationException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidDurationException(string message, string paramName, Exception innerException) :
        base(message, paramName, innerException)
        { }
    }
}