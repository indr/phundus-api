namespace Phundus.Tests.IdentityAccess.Model.Organizations
{
    using Events;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Organizations;

    [Subject(typeof (OrganizationContactDetailsChanged))]
    public class OrganizationContactDetailsChangedSpecs :
        identityaccess_domain_event_concern<OrganizationContactDetailsChanged>
    {
        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new OrganizationContactDetailsChanged(theManager, theOrganizationId, "Line1", "Line2", "Street",
                    "Postcode", "City", "PhoneNumber", "EmailAddress", "Website"));

        private It shold_have_at_2_the_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_10_the_website = () =>
            dataMember(10).ShouldEqual("Website");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_3_the_line_1 = () =>
            dataMember(3).ShouldEqual("Line1");

        private It should_have_at_4_the_line_2 = () =>
            dataMember(4).ShouldEqual("Line2");

        private It should_have_at_5_the_street = () =>
            dataMember(5).ShouldEqual("Street");

        private It should_have_at_6_the_postcode = () =>
            dataMember(6).ShouldEqual("Postcode");

        private It should_have_at_7_the_city = () =>
            dataMember(7).ShouldEqual("City");

        private It should_have_at_8_the_phone_number = () =>
            dataMember(8).ShouldEqual("PhoneNumber");

        private It should_have_at_9_the_email_address = () =>
            dataMember(9).ShouldEqual("EmailAddress");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.Organizations.OrganizationContactDetailsChanged");
    }
}