namespace Phundus.Tests.IdentityAccess.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Commands;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof(UnlockMemberHandler))]
    public class when_unlock_member_is_handled : identityaccess_command_handler_concern<UnlockMember, UnlockMemberHandler>
    {
        private static UserId theMemberId;
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            theOrganizationId = theOrganization.Id;
            theMemberId = new UserId();

            organizationRepository.setup(x => x.GetById(theOrganizationId)).Return(theOrganization);
            command = new UnlockMember(theInitiatorId, theOrganizationId, theMemberId);
        };

        private It should_tell_organization_to_lock_member = () =>
            theOrganization.WasToldTo(x => x.UnlockMember(theManager, theMemberId));
    }
}