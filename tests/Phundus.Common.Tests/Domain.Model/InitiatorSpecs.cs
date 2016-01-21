namespace Phundus.Common.Tests.Domain.Model
{
    using Common.Domain.Model;
    using Machine.Specifications;

    [Subject(typeof (Initiator))]
    public class when_serializing_an_initiator : serialization_object_concern<Initiator>
    {
        private static InitiatorId theInitiatorId;
        private static string theEmailAddress;
        private static string theFullName;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theEmailAddress = "initiator@test.phundus.ch";
            theFullName = "The Initiator";
            sut_factory.create_using(() =>
                new Initiator(theInitiatorId, theEmailAddress, theFullName));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Common");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Common.Domain.Model.Initiator");

        private It should_have_the_email_address_at_2 = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_the_full_name_at_3 = () =>
            dataMember(3).ShouldEqual(theFullName);

        private It should_have_the_initiator_id_at_1 = () =>
            dataMember(1).ShouldEqual(theInitiatorId.Id);
    }
}