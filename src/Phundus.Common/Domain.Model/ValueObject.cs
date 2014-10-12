namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ValueObject
    {
        /// <summary>
        /// When overriden in a derived class, returns all components of a value objects which constitute its identity.
        /// </summary>
        /// <returns>An ordered list of equality components.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            var vo = obj as ValueObject;
            return GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetEqualityComponents());
        }
    }

    public abstract class ComparableValueObject : ValueObject, IComparable
    {
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return 0;
            if (object.ReferenceEquals(null, obj)) return 1;
            if (GetType() != obj.GetType())
                throw new InvalidOperationException();
            return CompareTo(obj as ComparableValueObject);
        }

        protected abstract IEnumerable<IComparable> GetComparableComponents();

        protected IComparable AsNonGenericComparable<T>(IComparable<T> comparable)
        {
            return new NonGenericComparable<T>(comparable);
        }

        protected int CompareTo(ComparableValueObject other)
        {
            using (var thisComponents = GetComparableComponents().GetEnumerator())
            using (var otherComponents = other.GetComparableComponents().GetEnumerator())
            {
                while (true)
                {
                    var x = thisComponents.MoveNext();
                    var y = otherComponents.MoveNext();
                    if (x != y)
                        throw new InvalidOperationException();
                    if (x)
                    {
                        var c = thisComponents.Current.CompareTo(otherComponents.Current);
                        if (c != 0)
                            return c;
                    }
                    else
                    {
                        break;
                    }
                }
                return 0;
            }
        }

        private class NonGenericComparable<T> : IComparable
        {
            private readonly IComparable<T> comparable;

            public NonGenericComparable(IComparable<T> comparable)
            {
                this.comparable = comparable;
            }

            public int CompareTo(object obj)
            {
                if (object.ReferenceEquals(comparable, obj)) return 0;
                if (object.ReferenceEquals(null, obj))
                    throw new ArgumentNullException();
                return comparable.CompareTo((T) obj);
            }
        }
    }

    public abstract class ComparableValueObject<T> : ComparableValueObject, IComparable<T>
        where T : ComparableValueObject<T>
    {
        public int CompareTo(T other)
        {
            return base.CompareTo(other);
        }
    }
}