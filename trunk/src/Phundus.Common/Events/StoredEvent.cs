namespace Phundus.Common.Events
{
    using System;
    using Domain.Model;

    public class StoredEvent
    {
        private Guid _aggregateId;
        private Guid _eventGuid;
        private DateTime _occuredOnUtc;
        private byte[] _serialization;
        private string _typeName;

        public StoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization)
        {
            _eventGuid = eventGuid;
            _typeName = typeName;
            _occuredOnUtc = occuredOnUtc;
            //_aggregateId = ;
            _serialization = serialization;
        }

        protected StoredEvent()
        {
        }

        public virtual long EventId { get; protected set; }

        public virtual Guid EventGuid
        {
            get { return _eventGuid; }
            protected set { _eventGuid = value; }
        }

        public virtual string TypeName
        {
            get { return _typeName; }
            protected set { _typeName = value; }
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

        public DomainEvent ToDomainEvent(IEventSerializer serializer)
        {
            return ToDomainEvent<DomainEvent>(serializer);
        }

        public TDomainEvent ToDomainEvent<TDomainEvent>(IEventSerializer serializer)
            where TDomainEvent : DomainEvent
        {
            var domainEventType = default(Type);
            try
            {
                domainEventType = Type.GetType(_typeName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Class load error, because: {0}", ex));
            }

            return
                serializer.Deserialize<TDomainEvent>(EventGuid, OccuredOnUtc, Serialization);
        }
    }
}