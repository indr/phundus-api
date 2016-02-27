namespace Phundus.Tests.Shop.Application
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;

    [Subject(typeof (RemoveCartItemHandler))]
    public class when_remove_cart_item_is_handled : command_handler_concern<RemoveCartItem, RemoveCartItemHandler>
    {
        private static Cart theCart;
        private static CartItemId theCartItemId = new CartItemId();

        private Establish ctx = () =>
        {
            theCart = mock.partial<Cart>(new object[] {theInitiatorId});
            depends.on<ICartRepository>()
                .WhenToldTo(x => x.FindByUserGuid(new UserId(theInitiatorId.Id)))
                .Return(theCart);
            command = new RemoveCartItem(theInitiatorId, theCartItemId);
        };

        private It should_tell_cart_to_remove_item = () =>
            theCart.WasToldTo(x => x.RemoveItem(theCartItemId));
    }
}