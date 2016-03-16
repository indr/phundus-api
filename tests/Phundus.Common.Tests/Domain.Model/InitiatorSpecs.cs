namespace Phundus.Common.Tests.Domain.Model
{
    using Common.Domain.Model;
    using Machine.Specifications;

    [Subject(typeof (Initiator))]
    public class when_deserializing_an_initiator : serialization_object_concern<Initiator>
    {
        private static InitiatorId theInitiatorId = new InitiatorId();
        private static string theEmailAddress = "initiator@test.phundus.ch";
        private static string theFullName = "The Initiator";

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Initiator(theInitiatorId, theEmailAddress, theFullName));

        private It should_have_at_1_the_initiator_id = () =>
            dataMember(1).ShouldEqual(theInitiatorId.Id);

        private It should_have_at_2_the_email_address = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_at_3_the_full_name = () =>
            dataMember(3).ShouldEqual(theFullName);

        private It should_have_email_address = () =>
            sut.EmailAddress.ShouldEqual(theEmailAddress);

        private It should_have_full_name = () =>
            sut.FullName.ShouldEqual(theFullName);

        private It should_have_initiator_id = () =>
            sut.InitiatorId.ShouldEqual(theInitiatorId);
    }
}