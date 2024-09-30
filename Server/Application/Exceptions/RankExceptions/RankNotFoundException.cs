using System;

namespace Application.Exceptions
{
    public class RankNotFoundException : Exception
    {
        public RankNotFoundException(string message) : base(message)
        {
        }
    }
}
