namespace Phundus.IdentityAccess.Users.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Admin : ValueObject
    {
        public Admin(UserId userId, string emailAddress, string fullName)
        {
            UserId = userId;
            EmailAddress = emailAddress;
            FullName = fullName;
        }

        protected Admin()
        {
        }

        public UserId UserId { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string FullName { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
        }
    }
}