namespace Phundus.Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class Manager : ValueObject
    {
        public Manager(UserId userId, string emailAddress, string fullName)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            if (fullName == null) throw new ArgumentNullException("fullName");

            UserId = userId;
            EmailAddress = emailAddress;
            FullName = fullName;
        }

        protected Manager()
        {
        }

        public UserId UserId { get; protected set; }

        [DataMember(Order = 1)]
        protected Guid UserGuid
        {
            get { return UserId.Id; }
            set { UserId = new UserId(value); }
        }

        [DataMember(Order = 2)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 3)]
        public string FullName { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
        }
    }
}