namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Owner : ValueObject
    {
        private OwnerId _ownerId;
        private string _name;
        private OwnerType _type;

        public Owner(OwnerId ownerId, string name, OwnerType type)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (name == null) throw new ArgumentNullException("name");
            if (type == OwnerType.Unknown) throw new ArgumentException("Owner type must not be unknown.", "type");
            _ownerId = ownerId;
            _name = name;
            _type = type;
        }

        protected Owner()
        {
        }

        [DataMember(Order = 1)]
        public virtual OwnerId OwnerId
        {
            get { return _ownerId; }
            protected set { _ownerId = value; }
        }

        [DataMember(Order = 3)]
        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        [DataMember(Order = 2)]
        public virtual OwnerType Type
        {
            get { return _type; }
            protected set { _type = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OwnerId;
            yield return Name;
            yield return Type;
        }
    }
}