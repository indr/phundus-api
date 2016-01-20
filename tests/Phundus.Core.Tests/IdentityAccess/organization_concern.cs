namespace Phundus.Tests.IdentityAccess
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Users.Model;

    public class organization_concern : aggregate_concern<Organization>
    {
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            sut = new Organization(Guid.NewGuid(), "Organization name");
        };

        protected static User CreateUser()
        {
            return new User("user@test.phundus.ch", "1234", "Hans", "Müller", "Street", "1000", "City", "012 345 67 89",
                null);
        }
    }
}