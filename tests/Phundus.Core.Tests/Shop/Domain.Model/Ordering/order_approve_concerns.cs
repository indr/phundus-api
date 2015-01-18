namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using Core.Shop.Orders.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (Order))]
    public class when_approving_order : order_concern
    {
        private Because of = () => sut.Approve(_initiatorId.Id);

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Is.NotNull));
    }
}