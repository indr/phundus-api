namespace Phundus.Core.Ddd
{
    using System;

    public class StoredEvent
    {
        private Guid _aggregateId;
        private Guid _eventId;
        private string _name;
        private DateTime _occuredOnUtc;
        private byte[] _serialization;

        public StoredEvent(Guid eventId, DateTime occuredOnUtc, string name, byte[] serialization)
        {
            _eventId = eventId;
            _name = name;
            _occuredOnUtc = occuredOnUtc;
            //_aggregateId = ;
            _serialization = serialization;
        }

        protected StoredEvent()
        {
        }

        public virtual long Id { get; protected set; }

        public virtual Guid EventId
        {
            get { return _eventId; }
            protected set { _eventId = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public virtual DateTime OccuredOnUtc
        {
            get { return _occuredOnUtc; }
            protected set { _occuredOnUtc = value; }
        }

        public virtual Guid AggregateId
        {
            get { return _aggregateId; }
            protected set { _aggregateId = value; }
        }

        public virtual byte[] Serialization
        {
            get { return _serialization; }
            protected set { _serialization = value; }
        }
    }
}