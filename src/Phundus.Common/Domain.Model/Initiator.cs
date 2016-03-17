namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;

    public class Initiator : ValueObject
    {
        public Initiator(UserId userId, string emailAddress, string fullName)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            if (fullName == null) throw new ArgumentNullException("fullName");
            UserId = userId;
            EmailAddress = emailAddress;
            FullName = fullName;
        }

        protected Initiator()
        {
        }

        public UserId UserId { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string FullName { get; protected set; }

        public Actor ToActor()
        {
            return new Actor(UserId.Id, EmailAddress, FullName);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
        }
    }
}