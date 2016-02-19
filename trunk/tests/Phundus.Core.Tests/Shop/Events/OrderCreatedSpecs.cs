namespace Phundus.Tests.Shop.Model
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderCreated))]
    public class order_created : domain_event_concern<OrderCreated>
    {
        private static ShortOrderId theShortOrderId;
        private static OrderId theOrderId;
        private static Lessor theLessor;
        private static Lessee theLessee;

        private Establish ctx = () =>
        {
            theShortOrderId = new ShortOrderId(1234);
            theOrderId = new OrderId();
            theLessor = new Lessor(new LessorId(), "The lessor", true);
            theLessee = new Lessee(new LesseeId(), "Hans", "Muster", "Street", "12345", "City", "user@test.phundus.ch",
                "+1234567890", "123456");

            sut_factory.create_using(() =>
                new OrderCreated(theInitiator, theOrderId, theShortOrderId,
                    theLessor, theLessee));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_short_order_id = () =>
            dataMember(1).ShouldEqual(theShortOrderId.Id);

        private It should_have_at_2_the_order_id = () =>
            dataMember(2).ShouldEqual(theOrderId.Id);

        private It should_have_at_3_the_initiator = () =>
            dataMember(3).ShouldEqual(theInitiator);

        private It should_have_at_4_the_lessor = () =>
            dataMember(4).ShouldEqual(theLessor);

        private It should_have_at_5_the_lessee = () =>
            dataMember(5).ShouldEqual(theLessee);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderCreated");
    }
}