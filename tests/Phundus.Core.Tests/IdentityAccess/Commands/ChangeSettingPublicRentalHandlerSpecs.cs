namespace Phundus.Tests.IdentityAccess.Commands
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess;
    using Phundus.IdentityAccess.Authorization;
    using Phundus.IdentityAccess.Organizations.Commands;
    using Phundus.IdentityAccess.Organizations.Model;
    using Rhino.Mocks;

    [Subject(typeof (ChangeSettingPublicRentalHandler))]
    public class when_handling_change_setting_public_rental :
        identityaccess_command_handler_concern<ChangeSettingPublicRental, ChangeSettingPublicRentalHandler>
    {
        private static Organization theOrganization;
        private static bool theValue;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            theValue = true;

            organizationRepository.WhenToldTo(x => x.GetById(theOrganization.Id)).Return(theOrganization);
            command = new ChangeSettingPublicRental(theInitiatorId, theOrganization.Id, theValue);
        };

        private It should_tell_to_change_setting_public_rental = () =>
            theOrganization.WasToldTo(x => x.ChangeSettingPublicRental(theInitiator, theValue));

        private It should_authorize_initiator_to_manage_organization = () =>
            authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<ManageOrganization>.Matches(p => Equals(p.OrganizationId, theOrganization.Id))));

    }
}