using System;

namespace Application.Exceptions
{
    public class DutyNotFoundException : Exception
    {
        public DutyNotFoundException(string msg)
            : base(msg)
        { }
    }
}
