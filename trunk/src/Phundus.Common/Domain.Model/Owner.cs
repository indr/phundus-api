namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Owner : ValueObject
    {
        public Owner(OwnerId ownerId, string name, OwnerType type)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (name == null) throw new ArgumentNullException("name");
            if (type == OwnerType.Unknown) throw new ArgumentException("Owner type must not be unknown.", "type");
            OwnerId = ownerId;
            Name = name;
            Type = type;
        }

        protected Owner()
        {
        }

        [DataMember(Order = 1)]
        public OwnerId OwnerId { get; protected set; }

        [DataMember(Order = 3)]
        public string Name { get; protected set; }

        [DataMember(Order = 2)]
        public OwnerType Type { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OwnerId;
            yield return Name;
            yield return Type;
        }
    }
}