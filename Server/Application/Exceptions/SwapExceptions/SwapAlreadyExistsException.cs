using System;

namespace Application.Exceptions.SwapExceptions
{
    public class SwapAlreadyExistsException : Exception
    {
        public SwapAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
