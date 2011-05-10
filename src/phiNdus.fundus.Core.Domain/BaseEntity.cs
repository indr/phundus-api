namespace phiNdus.fundus.Core.Domain
{
    public class BaseEntity
    {
        public BaseEntity() : this(0)
        {
        }

        public BaseEntity(int id)
        {
            Id = id;
        }

        public virtual int Id { get; protected set; }

        public virtual int Version { get; protected set; }
    }
}