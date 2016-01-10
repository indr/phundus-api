namespace Phundus.Integration.IdentityAccess
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Manager : ValueObject
    {
        public Manager(Guid managerId, string fullName, string emailAddress)
        {
            AssertionConcern.AssertArgumentNotNull(managerId, "ManagerId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(fullName, "FullName must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(emailAddress, "EmailAddress must be provided.");

            ManagerId = managerId;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }

        public string FullName { get; private set; }

        public Guid ManagerId { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ManagerId;
        }
    }
}