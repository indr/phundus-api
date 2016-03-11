namespace Phundus.IdentityAccess.Model.Users
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class PasswordResetted : DomainEvent
    {
        public PasswordResetted(UserId userId, string firstName, string lastName, string emailAddress, string newPassword)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (firstName == null) throw new ArgumentNullException("firstName");
            if (lastName == null) throw new ArgumentNullException("lastName");
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            if (newPassword == null) throw new ArgumentNullException("newPassword");
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            NewPassword = newPassword;
        }

        [DataMember(Order = 1)]
        public UserId UserId { get; protected set; }

        [DataMember(Order = 2)]
        public string FirstName { get; protected set; }

        [DataMember(Order = 3)]
        public string LastName { get; protected set; }

        [DataMember(Order = 4)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 5)]
        public string NewPassword { get; set; }
    }
}