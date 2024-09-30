using System;

namespace Application.Exceptions
{
    public class BadPersonalRequestException : Exception
    {
        public BadPersonalRequestException(string message) : base(message)
        {
        }
    }
}
