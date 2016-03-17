namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserEmailAddressChanged : DomainEvent
    {
        public UserEmailAddressChanged(Initiator initiator, UserId userId, string oldEmailAddress,
            string newEmailAddress)
        {
            if (oldEmailAddress == null) throw new ArgumentNullException("oldEmailAddress");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            Initiator = initiator == null ? null : initiator.ToActor();
            UserId = userId.Id;
            OldEmailAddress = oldEmailAddress;
            NewEmailAddress = newEmailAddress;
        }

        protected UserEmailAddressChanged()
        {
        }

        [DataMember(Order = 1)]
        public Guid UserId { get; protected set; }

        [DataMember(Order = 2)]
        public string OldEmailAddress { get; protected set; }

        [DataMember(Order = 3)]
        public string NewEmailAddress { get; protected set; }

        [DataMember(Order = 4)]
        public Actor Initiator { get; protected set; }
    }
}