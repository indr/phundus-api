namespace Phundus.Common.Events
{
    using System;

    public class StoredEvent
    {
        private Guid _eventGuid;
        private DateTime _occuredOnUtc;
        private byte[] _serialization;
        private string _streamName;
        private long _streamVersion;
        private string _typeName;

        public StoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization)
        {
            _eventGuid = eventGuid;
            _typeName = typeName;
            _occuredOnUtc = occuredOnUtc;
            _serialization = serialization;
        }

        public StoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization,
            string streamName, long streamVersion) : this(eventGuid, occuredOnUtc, typeName, serialization)
        {
            _streamName = streamName;
            _streamVersion = streamVersion;
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

        public virtual long StreamVersion
        {
            get { return _streamVersion; }
            protected set { _streamVersion = value; }
        }
    }
}