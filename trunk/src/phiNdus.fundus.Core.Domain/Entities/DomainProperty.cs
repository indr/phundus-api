namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainProperty : BaseEntity
    {
        public DomainProperty()
        {
        }

        public DomainProperty(int id, string name, DomainPropertyType type) : base(id)
        {
            Name = name;
            Type = type;
        }

        public virtual string Name { get; protected set; }

        public virtual DomainPropertyType Type { get; protected set; }
    }
}