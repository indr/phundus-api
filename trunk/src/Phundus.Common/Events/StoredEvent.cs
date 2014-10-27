namespace Phundus.Common.Events
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Domain.Model;

    public class StoredEvent
    {
        private Guid _eventGuid;
        private DateTime _occuredOnUtc;
        private byte[] _serialization;
        private string _typeName;
        private string _streamName = null;
        private long? _streamVersion = null;

        public StoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization)
        {
            _eventGuid = eventGuid;
            _typeName = typeName;
            _occuredOnUtc = occuredOnUtc;
            _serialization = serialization;
        }

        public StoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization,
            EventStreamId eventStreamId) : this(eventGuid, occuredOnUtc, typeName, serialization)
        {
            _streamName = eventStreamId.StreamName;
            _streamVersion = eventStreamId.StreamVersion;
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

        /// <summary>
        /// Example: Phundus.Core.IdentityAccess.Model.UserRegistered, Phundus.Core
        /// </summary>
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

        public virtual byte[] Serialization
        {
            get { return _serialization; }
            protected set { _serialization = value; }
        }

        public virtual string StreamName
        {
            get { return _streamName; }
            protected set { _streamName = value; }
        }

        public virtual long? StreamVersion
        {
            get { return _streamVersion; }
            protected set { _streamVersion = value; }
        }
    }
}