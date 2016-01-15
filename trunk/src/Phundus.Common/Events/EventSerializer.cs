namespace Phundus.Common.Events
{
    using System;
    using System.IO;
    using System.Reflection;
    using Domain.Model;
    using ProtoBuf;

    public interface IEventSerializer
    {
        byte[] Serialize(DomainEvent domainEvent);

        object Deserialize(Type type, Guid eventGuid, DateTime occuredOnUtc, byte[] serialization);
        T Deserialize<T>(Guid eventGuid, DateTime occuredOnUtc, byte[] serialization) where T : DomainEvent;
    }

    public class EventSerializer : IEventSerializer
    {
        private static readonly PropertyInfo EventGuidProperty = typeof (DomainEvent).GetProperty("EventGuid");
        private static readonly PropertyInfo OccuredOnUtcProp = typeof (DomainEvent).GetProperty("OccuredOnUtc");

        public byte[] Serialize(DomainEvent domainEvent)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);
            return stream.ToArray();
        }

        public object Deserialize(Type type, Guid eventGuid, DateTime occuredOnUtc, byte[] serialization)
        {
            var instance = Serializer.NonGeneric.Deserialize(type, new MemoryStream(serialization));

            EventGuidProperty.SetValue(instance, eventGuid, null);
            OccuredOnUtcProp.SetValue(instance, occuredOnUtc, null);

            return instance;
        }

        public T Deserialize<T>(Guid eventGuid, DateTime occuredOnUtc, byte[] serialization) where T : DomainEvent
        {
            var stream = new MemoryStream(serialization);
            var instance = Serializer.Deserialize<T>(stream);

            EventGuidProperty.SetValue(instance, eventGuid, null);
            OccuredOnUtcProp.SetValue(instance, occuredOnUtc, null);

            return instance;
        }
    }
}