namespace Phundus.Tests.IdentityAccess.Events
{
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;

    [Subject(typeof (EmailTemplateChanged))]
    public class email_template_changed : identityaccess_domain_event_concern<EmailTemplateChanged>
    {
        private static string orderReceivedText = "order received text";

        private Establish ctx = () => sut_factory.create_using(() =>
            new EmailTemplateChanged(theManager, theOrganizationId, orderReceivedText));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_order_received_text = () =>
            dataMember(3).ShouldEqual(orderReceivedText);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.EmailTemplateChanged");
    }
}