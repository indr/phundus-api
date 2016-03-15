namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Founder : ValueObject
    {
        public Founder(UserId userId, string emailAddress, string fullName)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            if (fullName == null) throw new ArgumentNullException("fullName");
            UserId = userId;
            EmailAddress = emailAddress;
            FullName = fullName;
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