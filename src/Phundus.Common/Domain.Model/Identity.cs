namespace Phundus.Common.Domain.Model
{
    using System.Collections.Generic;

    public abstract class Identity<T>
    {
        protected Identity(T value)
        {
            AssertionConcern.AssertArgumentNotNull(value, "Value must be provided.");

            Value = value;
        }

        protected Identity()
        {
        } 

        public T Value { get; private set; }

        protected bool Equals(Identity<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identity<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }
    }
}