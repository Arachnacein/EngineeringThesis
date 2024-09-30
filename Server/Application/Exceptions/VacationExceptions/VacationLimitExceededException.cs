using System;

namespace Application.Exceptions.Vacation
{
    public class VacationLimitExceededException : Exception
    {
        public VacationLimitExceededException(string message) : base(message)
        {}
    }
}
