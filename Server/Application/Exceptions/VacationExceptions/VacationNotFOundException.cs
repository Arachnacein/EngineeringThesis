using System;

namespace Application.Exceptions
{
    public class VacationNotFOundException : Exception
    {
        public VacationNotFOundException(string msg) : base(msg)
        { }
    }
}
