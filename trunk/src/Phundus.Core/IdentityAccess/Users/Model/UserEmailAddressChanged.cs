namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserEmailAddressChanged : DomainEvent
    {
        public UserEmailAddressChanged(Guid userGuid, string oldEmailAddress, string newEmailAddress)
        {
            if (oldEmailAddress == null) throw new ArgumentNullException("oldEmailAddress");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");
            UserGuid = userGuid;
            OldEmailAddress = oldEmailAddress;
            NewEmailAddress = newEmailAddress;
        }

        protected UserEmailAddressChanged()
        {
        }

        [DataMember(Order = 1)]
        public Guid UserGuid { get; set; }

        [DataMember(Order = 2)]
        public string OldEmailAddress { get; protected set; }

        [DataMember(Order = 3)]
        public string NewEmailAddress { get; protected set; }
    }
}