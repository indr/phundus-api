namespace Phundus.Tests.IdentityAccess.Authorize
{
    using Authorization;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Authorization;
    using Phundus.IdentityAccess.Queries;

    public class identityaccess_access_object_handler_concern<TAccessObject, TAccessObjectHandler> :
        access_object_handler_concern<TAccessObject, TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static identityaccess_factory make;
        protected static IMemberInRole memberInRole;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            make = new identityaccess_factory(fake);
        };
    }

    [Subject(typeof (ManageOrganizationAccessObjectHandler))]
    public class when_handling_manage_organization :
        identityaccess_access_object_handler_concern<ManageOrganization, ManageOrganizationAccessObjectHandler>
    {
        private static OrganizationId theOrganizationId;
        private Establish ctx = () =>
        {
            theOrganizationId = new OrganizationId();
            theAccessObject = new ManageOrganization(theOrganizationId);
        };

        private It should_tell_member_in_role_active_manager = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(theOrganizationId, theUserId));
    }
}