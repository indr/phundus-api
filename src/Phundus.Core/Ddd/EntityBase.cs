namespace Phundus.Ddd
{
    /// <summary>
    ///   Basisklasse für alle Entitäten im Domain Model.
    /// </summary>
    public class EntityBase
    {
        private int _id;
        private int _version;

        protected EntityBase() : this(0)
        {
        }

        protected EntityBase(int id) : this(id, 0)
        {
        }

        protected EntityBase(int id, int version)
        {
            _id = id;
            _version = version;
        }

        /// <summary>
        ///   Die künstliche Id des Objektes.
        /// </summary>
        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        /// <summary>
        ///   Die Version des Objektes. Wird von NHibernate für Optimistic Offline Locking benötigt.
        /// </summary>
        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }
    }
}