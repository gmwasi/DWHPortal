using DWHDashboard.SharedKernel.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWHDashboard.SharedKernel
{
    public abstract class Entity
    {
        [Key, Column(Order = 1)]
        public virtual Guid Id { get; set; }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity() : this(LiveGuid.NewGuid())
        {
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if ((Id == Guid.Empty) || (other.Id == Guid.Empty))
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}