namespace phiNdus.fundus.Core.Domain.Entities
{
    public class BaseEntity
    {
        protected BaseEntity() : this(0)
        {
            
        }

        protected BaseEntity(int id)
        {
            _id = id;
        }

        private int _id;
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