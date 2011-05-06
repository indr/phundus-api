namespace phiNdus.fundus.Core.Domain
{
    public class BaseEntity
    {
        private int _id;

        public BaseEntity() : this(0)
        {
        }

        public BaseEntity(int id)
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