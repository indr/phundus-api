namespace phiNdus.fundus.Core.Domain.Entities
{
    public class BaseEntity
    {
        private int _id;

        protected BaseEntity() : this(0)
        {
        }

        protected BaseEntity(int id)
        {
            _id = id;
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version { get; protected set; }
    }
}