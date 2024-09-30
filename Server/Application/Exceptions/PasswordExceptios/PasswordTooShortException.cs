using System;

namespace Application.Exceptions
{
    public class PasswordTooShortException : Exception
    {
        public PasswordTooShortException(string message) : base(message)
        {
        }
    }
}
