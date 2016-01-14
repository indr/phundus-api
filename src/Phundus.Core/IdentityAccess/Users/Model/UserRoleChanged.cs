namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserRoleChanged : DomainEvent
    {
        public UserRoleChanged(User initiator, User user, UserRole oldRole, UserRole newRole)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (user == null) throw new ArgumentNullException("user");
        }

        protected UserRoleChanged()
        {
        }
    }
}