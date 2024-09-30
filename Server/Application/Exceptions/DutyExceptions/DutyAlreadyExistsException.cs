using System;

namespace Application.Exceptions.DutyExceptions
{
    public class DutyAlreadyExistsException : Exception
    {
        public DutyAlreadyExistsException(string message) : base(message)
        {
        }
    }
}