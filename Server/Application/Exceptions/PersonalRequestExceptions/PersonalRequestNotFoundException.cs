using System;

namespace Application.Exceptions
{
    public class PersonalRequestNotFoundException : Exception
    {
        public PersonalRequestNotFoundException(string msg) : base(msg)
        { } 
    }
}
