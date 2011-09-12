namespace phiNdus.fundus.Core.Domain.Entities
{
    public class BaseEntity
    {
        private int _id;

        protected BaseEntity() : this(0)
        {
        }

        protected BaseEntity(int id) : this(id, 0)
        {
        }

        protected BaseEntity(int id, int version)
        {
            _id = id;
            _version = version;
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        private int _version;
        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }
    }
}