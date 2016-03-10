namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using IdentityAccess.Model.Users;

    [DataContract]
    public class UserRoleChanged : DomainEvent
    {
        public UserRoleChanged(Admin admin, User user, UserRole oldRole, UserRole newRole)
        {
            if (admin == null) throw new ArgumentNullException("admin");
            if (user == null) throw new ArgumentNullException("user");
        }

        protected UserRoleChanged()
        {
        }
    }
}