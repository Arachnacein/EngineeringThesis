using System;

namespace Application.Exceptions
{
    public class BadValueException : Exception
    {
        public BadValueException(string message) : base(message)
        {
        }
    }
}
