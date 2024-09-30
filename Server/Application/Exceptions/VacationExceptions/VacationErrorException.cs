using System;

namespace Application.Exceptions.Vacation
{
    public class VacationErrorException : Exception
    {
        public VacationErrorException(string message) : base(message)
        {
        }
    }
}
