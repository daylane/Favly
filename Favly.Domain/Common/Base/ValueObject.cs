using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Base
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
        public override int GetHashCode()
        {
           return GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : 0).Aggregate((a, b) => a ^ b);
        }
    }
}
