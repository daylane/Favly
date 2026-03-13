using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public static void When(bool notFound, string message)
        {
            if (notFound)
                throw new NotFoundException(message);
        }
    }
}
