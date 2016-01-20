namespace Phundus.Tests.IdentityAccess.Commands
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Commands;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (ChangeSettingPublicRentalHandler))]
    public class when_handling_change_setting_public_rental :
        identityaccess_handler_concern<ChangeSettingPublicRental, ChangeSettingPublicRentalHandler>
    {
        private static Organization theOrganization;
        private static bool theValue;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            theValue = true;

            memberInRole.WhenToldTo(x => x.ActiveManager(theOrganization.Id, theInitiatorId)).Return(theInitiator);
            organizationRepository.WhenToldTo(x => x.GetById(theOrganization.Id)).Return(theOrganization);
            command = new ChangeSettingPublicRental(theInitiatorId, theOrganization.Id, theValue);
        };

        private It should_ask_for_manager_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(theOrganization.Id, theInitiatorId));

        private It should_tell_to_change_setting_public_rental = () =>
            theOrganization.WasToldTo(x => x.ChangeSettingPublicRental(theInitiator, theValue));
    }
}