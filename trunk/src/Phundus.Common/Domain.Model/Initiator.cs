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
            InitiatorGuid = initiatorId.Id;
            EmailAddress = emailAddress;
            FullName = fullName;
        }

        protected Initiator()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorGuid { get; protected set; }

        public InitiatorId InitiatorId
        {
            get { return new InitiatorId(InitiatorGuid); }
        }

        [DataMember(Order = 2)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 3)]
        public string FullName { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return InitiatorGuid;
        }
    }
}