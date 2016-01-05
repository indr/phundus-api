namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserApproved : DomainEvent
    {
        public UserApproved(User initiator, User user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (user == null) throw new ArgumentNullException("user");
        }

        protected UserApproved()
        {
        }
    }
}