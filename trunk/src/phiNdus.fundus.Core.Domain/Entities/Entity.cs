namespace phiNdus.fundus.Core.Domain.Entities
{
    /// <summary>
    ///   Basisklasse für alle Entitäten im Domain Model.
    /// </summary>
    public class Entity
    {
        private int _id;
        private int _version;

        protected Entity() : this(0)
        {
        }

        protected Entity(int id) : this(id, 0)
        {
        }

        protected Entity(int id, int version)
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