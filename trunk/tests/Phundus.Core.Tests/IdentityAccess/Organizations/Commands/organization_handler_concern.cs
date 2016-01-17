namespace Phundus.Tests.IdentityAccess.Organizations.Commands
{
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Commands;

    public class organization_handler_concern :
        handler_concern<ChangeOrganizationContactDetails, ChangeOrganizationContactDetailsHandler>
    {
        private Establish ctx = () =>
            make = new identityaccess_factory(fake);

        protected static identityaccess_factory make;
    }
}