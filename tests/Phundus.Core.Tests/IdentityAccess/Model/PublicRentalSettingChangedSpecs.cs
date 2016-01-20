namespace Phundus.Tests.IdentityAccess.Model
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;

    [Subject(typeof (PublicRentalSettingChanged))]
    public class public_rental_setting_changed : domain_event_concern<PublicRentalSettingChanged>
    {
        private static OrganizationId theOrganizationId;
        private static bool theValue;

        private Establish ctx = () =>
        {
            theOrganizationId = new OrganizationId();
            theValue = true;
            sut_factory.create_using(() => new PublicRentalSettingChanged(theInitiator, theOrganizationId, theValue));
        };

        private It should_be_in_namespace = () =>
            itsNamespace.ShouldEqual("Phundus.IdentityAccess.Model");

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_initiator_at_1 = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_organization_id_at_2 = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_value_at_3 = () =>
            dataMember(3).ShouldEqual(theValue);
    }
}