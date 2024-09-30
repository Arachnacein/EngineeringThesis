using System;

namespace Application.Exceptions
{
    public class PersonalRequestAlreadyExistsException : Exception
    {
        public PersonalRequestAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
