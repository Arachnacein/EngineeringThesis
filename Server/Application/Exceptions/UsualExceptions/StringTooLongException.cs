using System;

namespace Application.Exceptions
{
    public class StringTooLongException : Exception
    {
        public StringTooLongException(string message) : base(message)
        {
        }
    }
}
