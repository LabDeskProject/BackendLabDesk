using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace LabDesk.SeedWork.Domain
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; } = default!; //Avoid Nullable Warning
        public DateTimeOffset CreatedAt { get; protected set; }
        public DateTimeOffset? UpdatedAt { get; protected set; }
        protected Entity(TId id)
        {
            Id = id;
        }

        protected void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        protected Entity() { } //EF Core

        // Compare Entity by ID
        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (EqualityComparer<TId>.Default.Equals(Id, default!) || EqualityComparer<TId>.Default.Equals(other.Id, default!)) return false;

            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !(left == right);
        }
    }
}
