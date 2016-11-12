namespace Phundus.Tests.IdentityAccess.Events
{
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;

    [Subject(typeof (PublicRentalSettingChanged))]
    public class public_rental_setting_changed : identityaccess_domain_event_concern<PublicRentalSettingChanged>
    {
        private static bool theValue = true;

        private Establish ctx = () => sut_factory.create_using(() =>
            new PublicRentalSettingChanged(theManager, theOrganizationId, theValue));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_value = () =>
            dataMember(3).ShouldEqual(theValue);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.PublicRentalSettingChanged");
    }
}