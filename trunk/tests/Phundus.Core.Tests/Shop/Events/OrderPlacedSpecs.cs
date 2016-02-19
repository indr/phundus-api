namespace Phundus.Tests.Shop.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderPlaced))]
    public class order_placed : domain_event_concern<OrderPlaced>
    {
        private static ShortOrderId theShortOrderId;
        private static Lessor theLessor;
        private static OrderId theOrderId;
        private static Lessee theLessee;

        private Establish ctx = () =>
        {
            var make = new shop_factory(fake);

            theShortOrderId = new ShortOrderId(1234);
            theOrderId = new OrderId();
            theLessor = new Lessor(new LessorId(), "The lessor", true);
            theLessee = new Lessee(new LesseeId(), "First name", "Last name", "Street", "Postcode", "City",
                "Email address", "Phone number", "Member number");

            sut_factory.create_using(() =>
                new OrderPlaced(theInitiator, theOrderId, theShortOrderId,
                    theLessor, theLessee));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_short_order_id = () =>
            dataMember(1).ShouldEqual(theShortOrderId.Id);

        private It should_have_at_2_the_lessor_id = () =>
            dataMember(2).ShouldEqual(theLessor.LessorId.Id);

        private It should_have_at_3_the_order_id = () =>
            dataMember(3).ShouldEqual(theOrderId.Id);

        private It should_have_at_4_the_initiator = () =>
            dataMember(4).ShouldEqual(theInitiator);

        private It should_have_at_5_the_lessor = () =>
            dataMember(5).ShouldEqual(theLessor);

        private It should_have_at_6_the_lessee = () =>
            dataMember(6).ShouldEqual(theLessee);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderPlaced");

    }
}