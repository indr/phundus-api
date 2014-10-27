namespace Phundus.Common.Events
{
    using System;
    using System.IO;
    using System.Reflection;
    using Domain.Model;
    using ProtoBuf;

    public interface IEventSerializer
    {
        byte[] Serialize(IDomainEvent domainEvent);

        object Deserialize(Type type, Guid id, DateTime occuredOnUtc, byte[] serialization);
        T Deserialize<T>(Guid id, DateTime occuredOnUtc, byte[] serialization) where T : DomainEvent;
    }

    public class EventSerializer : IEventSerializer
    {
        private static readonly PropertyInfo IdProperty = typeof (DomainEvent).GetProperty("Id");
        private static readonly PropertyInfo OccuredOnUtcProp = typeof (DomainEvent).GetProperty("OccuredOnUtc");

        public byte[] Serialize(IDomainEvent domainEvent)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);
            return stream.ToArray();
        }

        public object Deserialize(Type type, Guid id, DateTime occuredOnUtc, byte[] serialization)
        {
            var instance = Serializer.NonGeneric.Deserialize(type, new MemoryStream(serialization));

            IdProperty.SetValue(instance, id, null);
            OccuredOnUtcProp.SetValue(instance, occuredOnUtc, null);

            return instance;
        }

        public T Deserialize<T>(Guid id, DateTime occuredOnUtc, byte[] serialization) where T : DomainEvent
        {
            var stream = new MemoryStream(serialization);
            var instance = Serializer.Deserialize<T>(stream);

            IdProperty.SetValue(instance, id, null);
            OccuredOnUtcProp.SetValue(instance, occuredOnUtc, null);

            return instance;
        }
    }
}