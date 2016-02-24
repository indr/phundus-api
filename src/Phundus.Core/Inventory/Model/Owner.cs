namespace Phundus.Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

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

        public virtual OwnerId OwnerId
        {
            get { return _ownerId; }
            protected set { _ownerId = value; }
        }

        [DataMember(Order = 1)]
        protected virtual Guid OwnerGuid
        {
            get { return OwnerId.Id; }
            set { OwnerId = new OwnerId(value);}
        }

        [DataMember(Order = 2)]
        public virtual OwnerType Type
        {
            get { return _type; }
            protected set { _type = value; }
        }

        [DataMember(Order = 3)]
        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OwnerId;
        }
    }

    public enum OwnerType
    {
        Unknown,
        Organization,
        User
    }
}