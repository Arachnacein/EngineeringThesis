using System;

namespace Application.Exceptions
{
    public class SwapNotFoundException : Exception
    {
        public SwapNotFoundException(string msg)
    : base(msg)
        { }
    }
}
