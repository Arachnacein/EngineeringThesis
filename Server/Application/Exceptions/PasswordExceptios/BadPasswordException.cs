using System;

namespace Application.Exceptions
{
    public class BadPasswordException : Exception
    {
        public BadPasswordException(string message) : base(message)
        {
        }
    }
}