using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Domain
{
    // Compare value like: Money, Address, RequestNumber,... Instead of comparing ID
    public abstract class ValueObject : IEquatable<ValueObject>
    {

        protected abstract IEnumerable<object?> GetAtomicValues();
        public bool Equals(ValueObject? other)
        {
            return other is not null && ValuesAreEqual(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is ValueObject other && ValuesAreEqual(other);
        }

        private bool ValuesAreEqual(ValueObject other)
        {
            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Aggregate(default(int), (hashcode, value) =>
                    HashCode.Combine(hashcode, value?.GetHashCode() ?? 0));
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !(left == right);
        }
    }
}
