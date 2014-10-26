﻿namespace Phundus.Common.Domain.Model
{
    using System;

    public abstract class Identity<T> : IEquatable<Identity<T>>, IIdentity<T>
    {
        protected Identity(T id)
        {
            Id = id;
        }

        public bool Equals(Identity<T> id)
        {
            if (ReferenceEquals(this, id)) return true;
            if (ReferenceEquals(null, id)) return false;
            return Id.Equals(id.Id);
        }

        public T Id { get; protected set; }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity<T>);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode()*907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}