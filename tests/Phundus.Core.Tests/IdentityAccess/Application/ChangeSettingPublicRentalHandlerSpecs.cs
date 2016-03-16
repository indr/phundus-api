namespace Phundus.Tests.IdentityAccess.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (ChangeSettingPublicRentalHandler))]
    public class when_handling_change_setting_public_rental :
        identityaccess_command_handler_concern<ChangeSettingPublicRental, ChangeSettingPublicRentalHandler>
    {
        private static Organization theOrganization;
        private static bool theValue;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization(theOrganizationId);
            theValue = true;

            organizationRepository.setup(x => x.GetById(theOrganization.Id)).Return(theOrganization);
            command = new ChangeSettingPublicRental(theInitiatorId, theOrganization.Id, theValue);
        };

        private It should_change_setting_public_rental = () =>
            theOrganization.received(x => x.ChangeSettingPublicRental(theManager, theValue));
    }
}