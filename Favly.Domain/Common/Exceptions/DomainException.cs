using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
        
        public static void When(bool hasError, string message)
        {
            if (hasError)
            {
                throw new DomainException(message);
            }
        }
    }
}
