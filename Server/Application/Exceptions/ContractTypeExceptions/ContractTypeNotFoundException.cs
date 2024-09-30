using System;

namespace Application.Exceptions
{
    public class ContractTypeNotFoundException : Exception
    {
        public ContractTypeNotFoundException(string message) : base(message)
        {
        }
    }
}
