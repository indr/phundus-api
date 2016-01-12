namespace Phundus.Core.Tests.Shop.Commands
{
    using Common.Domain.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof(RemoveCartItemHandler))]
    public class when_remove_cart_item_is_handled : handler_concern<RemoveCartItem, RemoveCartItemHandler>
    {
        private Establish ctx = () =>
        {
            theInitiatorId = new UserId(1001);
            theCart = mock.partial<Cart>(new object[] {theInitiatorId});
            depends.on<ICartRepository>().WhenToldTo(x => x.FindByUserId(theInitiatorId)).Return(theCart);
            command = new RemoveCartItem(theInitiatorId, theCartItemId);
        };

        private static UserId theInitiatorId = new UserId(1001);
        private static Cart theCart;
        private static CartItemId theCartItemId = new CartItemId();

        private It should_tell_cart_to_remove_item = () => theCart.WasToldTo(x => x.RemoveItem(theCartItemId));
    }
}