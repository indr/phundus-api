namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainPropertyDefinition : BaseEntity
    {
        private DomainPropertyType _dataType;
        private string _name;

        public DomainPropertyDefinition()
        {
        }

        public DomainPropertyDefinition(DomainPropertyType type)
            : this(0, "", type)
        {
        }

        public DomainPropertyDefinition(int id, string name, DomainPropertyType type) : this(id, 0, name, type)
        {
        }

        public DomainPropertyDefinition(int id, int version, string name, DomainPropertyType type)
            : this(id, version, name, type, false)
        {
        }

        public DomainPropertyDefinition(int id, int version, string name, DomainPropertyType type, bool isSystemProperty)
            : base(id, version)
        {
            _name = name;
            _dataType = type;
            IsSystemProperty = isSystemProperty;
        }

        // According to dml.sql
        public static int VerfuegbarId
        {
            get { return 1; }
        }

        public static int CaptionId
        {
            get { return 2; }
        }

        public static int StockId
        {
            get { return 3; }
        }

        public static int PriceId
        {
            get { return 4; }
        }

        public static int ErfassungsdatumId
        {
            get { return 5; }
        }

        public static int IsReservableId
        {
            get { return 6; }
        }

        public static int IsBorrowableId
        {
            get { return 7; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual DomainPropertyType DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public virtual bool IsSystemProperty { get; protected set; }
    }
}