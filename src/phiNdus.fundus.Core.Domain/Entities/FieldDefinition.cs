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

        public FieldDefinition(int id, int version, string name, DataType type, bool isSystem)
            : base(id, version)
        {
            _name = name;
            _dataType = type;
            _isSystem = isSystem;
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

        private bool _isSystem;

        /// <summary>
        /// Gibt an, ob das Feld systemrelevant ist und nicht entfernt
        /// werden kann (z.B. Menge, Preis usw.).
        /// </summary>
        public virtual bool IsSystem
        {
            get { return _isSystem; }
            protected set { _isSystem = value; }
        }

        private bool _isDefault;
        
        /// <summary>
        /// Angabe ob das Feld bei der Erfassung eines neuen Artikels
        /// automatisch hinzugefügt wird.
        /// </summary>
        public virtual bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }

        private int _position;

        /// <summary>
        /// Die Anzeigeposition beim Erfassen, Bearbeiten und Anzeigen
        /// eines Artikels.
        /// </summary>
        public virtual int Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}