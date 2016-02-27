namespace Phundus.Tests.IdentityAccess.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (ChangeMemberRecievesEmailNotification))]
    public class when_change_member_recieves_email_notification_is_handled :
        identityaccess_command_handler_concern
            <ChangeMemberRecievesEmailNotification, ChangeMemberRecievesEmailNotificationHandler>
    {
        private static Organization theOrganization;
        private static UserId theMemberId;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            theMemberId = new UserId();
            organizationRepository.setup(x => x.GetById(theOrganizationId)).Return(theOrganization);

            command = new ChangeMemberRecievesEmailNotification(theInitiatorId, theOrganizationId, theMemberId, true);
        };

        private It should_tell_organization_to_change_members_recieve_email_notification_option = () =>
            theOrganization.WasToldTo(x => x.ChangeMembersRecieveEmailNotificationOption(theManager, theMemberId, true));
    }
}