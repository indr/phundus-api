namespace Phundus.Tests.IdentityAccess.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;

    public class identityaccess_domain_event_concern<TDomainEvent> : domain_event_concern<TDomainEvent>
        where TDomainEvent : class
    {
        protected static Admin theAdmin;
        protected static Manager theManager;
        protected static OrganizationId theOrganizationId = new OrganizationId();

        private Establish ctx = () =>
        {
            var make = new identityaccess_factory(fake);
            theAdmin = make.Admin(theInitiatorId);
            theManager = make.Manager(theInitiatorId);
        };
    }
}