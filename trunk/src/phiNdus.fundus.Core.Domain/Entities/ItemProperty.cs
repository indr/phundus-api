namespace phiNdus.fundus.Core.Domain.Entities
{
    public class ItemProperty : BaseEntity
    {
        public ItemProperty()
        {
        }

        public ItemProperty(int id, string name, ItemPropertyType type) : base(id)
        {
            Name = name;
            Type = type;
        }

        public virtual string Name { get; protected set; }

        public virtual ItemPropertyType Type { get; protected set; }
    }
}