namespace Phundus.Core.Entities
{
    public class FieldDefinition : EntityBase
    {
        public static int VerfuegbarId = 1;
        public static int CaptionId = 2;
        public static int PriceId = 4;
        public static int CreateDateId = 5;
        public static int IsReservableId = 6;
        public static int IsBorrowableId = 7;
        public static int GrossStockId = 10;
        public static int NetStockId = 11;

        private DataType _dataType;
        private bool _isAttachable = true;
        private bool _isSystem;
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

        /// <summary>
        /// Gibt an, ob das Feld systemrelevant ist und nicht entfernt
        /// werden kann (z.B. Menge, Preis usw.).
        /// </summary>
        public virtual bool IsSystem
        {
            get { return _isSystem; }
            protected set { _isSystem = value; }
        }

        /// <summary>
        /// Gibt an, ob das Feld vom Benutzer einem Material hinzugefügt
        /// werden kann. (z.B. Netto-Bestand darf nicht hinzugefügt werden usw.)
        /// </summary>
        public virtual bool IsAttachable
        {
            get { return _isAttachable; }
            protected set { _isAttachable = value; }
        }

        /// <summary>
        /// Angabe ob das Feld bei der Erfassung eines neuen Artikels
        /// automatisch hinzugefügt wird.
        /// </summary>
        public virtual bool IsDefault { get; set; }


        /// <summary>
        /// Angabe ob das Feld in Tabellen als Spalte angezeigt wird.
        /// </summary>
        public virtual bool IsColumn { get; set; }

        /// <summary>
        /// Die Anzeigeposition beim Erfassen, Bearbeiten und Anzeigen
        /// eines Artikels.
        /// </summary>
        public virtual int Position { get; set; }
    }
}