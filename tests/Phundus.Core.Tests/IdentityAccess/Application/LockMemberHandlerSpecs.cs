namespace Phundus.Tests.IdentityAccess.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (LockMemberHandler))]
    public class when_lock_member_is_handled : identityaccess_command_handler_concern<LockMember, LockMemberHandler>
    {
        private static UserId theMemberId;
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            theOrganizationId = theOrganization.Id;
            theMemberId = new UserId();

            userInRoleService.setup(x => x.Manager(theInitiatorId, theOrganizationId)).Return(theManager);
            organizationRepository.setup(x => x.GetById(theOrganizationId)).Return(theOrganization);
            command = new LockMember(theInitiatorId, theOrganizationId, theMemberId);
        };

        private It should_tell_organization_to_lock_member = () =>
            theOrganization.WasToldTo(x => x.LockMember(theManager, theMemberId));
    }
}