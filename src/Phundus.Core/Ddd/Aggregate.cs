namespace Phundus.Ddd
{
    using System;
    using Common;
    
    public abstract class Aggregate<TIdentity>
    {
        private DateTime _createdAtUtc;
        private TIdentity _id;
        private DateTime _modifiedAtUtc;
        private int _version;

        protected Aggregate(TIdentity id)
        {
            AssertionConcern.AssertArgumentNotNull(id, "Id must be provided.");

            _id = id;
            _version = 0;
            _createdAtUtc = DateTime.UtcNow;
            _modifiedAtUtc = _createdAtUtc;
        }

        protected Aggregate()
        {
        }

        public virtual TIdentity Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual DateTime CreatedAtUtc
        {
            get { return _createdAtUtc; }
            protected set { _createdAtUtc = value; }
        }

        public virtual DateTime ModifiedAtUtc
        {
            get { return _modifiedAtUtc; }
            protected set { _modifiedAtUtc = value; }
        }
    }
}