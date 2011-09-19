namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainPropertyDefinition : BaseEntity
    {
        // According to dml.sql
        public static int VerfuegbarId { get { return 1; } }
        public static int CaptionId { get { return 2; } }
        public static int StockId { get { return 3; } }
        public static int PriceId { get { return 4; } }
        public static int ErfassungsdatumId { get { return 5; } }
        public static int IsReservableId { get { return 6; } }
        public static int IsLendableId { get { return 7; } }

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