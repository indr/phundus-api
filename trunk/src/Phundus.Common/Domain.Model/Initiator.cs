namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Initiator : ValueObject
    {
        public Initiator(InitiatorId initiatorId, string emailAddress, string fullName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (fullName == null) throw new ArgumentNullException("fullName");
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            InitiatorId = initiatorId.Id;
            EmailAddress = emailAddress;
            FullName = fullName;
        }

        [DataMember(Order = 1)]
        public Guid InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 3)]
        public string FullName { get; protected set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return InitiatorId;
        }
    }
}