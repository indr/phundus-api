namespace Phundus.IdentityAccess.Model.Users
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class EmailAddressChangeRequested : DomainEvent
    {
        public EmailAddressChangeRequested(Initiator initiator, UserId userId, string firstName, string lastName, string requestedEmailAddress, string validationKey)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (userId == null) throw new ArgumentNullException("userId");
            if (requestedEmailAddress == null) throw new ArgumentNullException("requestedEmailAddress");
            if (validationKey == null) throw new ArgumentNullException("validationKey");            
            
            Initiator = initiator.ToActor();
            UserId = userId.Id;
            FirstName = firstName;
            LastName = lastName;
            RequestedEmailAddress = requestedEmailAddress;
            ValidationKey = validationKey;
        }

        protected EmailAddressChangeRequested()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserId { get; protected set; }
        
        [DataMember(Order = 3)]
        public string FirstName { get; protected set; }

        [DataMember(Order = 4)]
        public string LastName { get; protected set; }

        [DataMember(Order = 5)]
        public string RequestedEmailAddress { get; protected set; }

        [DataMember(Order = 6)]
        public string ValidationKey { get; protected set; }
    }
}