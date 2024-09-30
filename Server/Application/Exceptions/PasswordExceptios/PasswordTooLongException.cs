using System;

namespace Application.Exceptions
{
    public class PasswordTooLongException : Exception
    {
        public PasswordTooLongException(string message) : base(message)
        {
        }
    }
}
