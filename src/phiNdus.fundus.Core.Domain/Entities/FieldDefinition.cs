namespace phiNdus.fundus.Core.Domain.Entities
{
    public class FieldDefinition : Entity
    {
        private DataType _dataType;
        private string _name;

        public FieldDefinition()
        {
        }

        public FieldDefinition(DataType type)
            : this(0, "", type)
        {
        }

        public FieldDefinition(int id, string name, DataType type) : this(id, 0, name, type)
        {
        }

        public FieldDefinition(int id, int version, string name, DataType type)
            : this(id, version, name, type, false)
        {
        }

        public FieldDefinition(int id, int version, string name, DataType type, bool isSystemField)
            : base(id, version)
        {
            _name = name;
            _dataType = type;
            _isSystemField = isSystemField;
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

        public static int GrossStockId
        {
            get { return 10; }
        }

        public static int NetStockId
        {
            get { return 11; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual DataType DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        private bool _isSystemField;
        public virtual bool IsSystemField
        {
            get { return _isSystemField; }
            protected set { _isSystemField = value; }
        }
    }
}