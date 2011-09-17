namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainPropertyDefinition : BaseEntity
    {
        public DomainPropertyDefinition()
        {
        }

        public DomainPropertyDefinition(DomainPropertyType type)
            : this(0, "", type)
        {
        }

        public DomainPropertyDefinition(int id, string name, DomainPropertyType type) : base(id)
        {
            _name = name;
            _dataType = type;
        }

        private string _name;
        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        private DomainPropertyType _dataType;
        public virtual DomainPropertyType DataType
        {
            get { return _dataType; }
            protected set { _dataType = value; }
        }
    }
}