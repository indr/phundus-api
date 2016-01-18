namespace Phundus.Tests.Shop.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;

    [Subject(typeof (RemoveCartItemHandler))]
    public class when_remove_cart_item_is_handled : handler_concern<RemoveCartItem, RemoveCartItemHandler>
    {
        private static Cart theCart;
        private static CartItemGuid theCartItemId = new CartItemGuid();

        private Establish ctx = () =>
        {
            theCart = mock.partial<Cart>(new object[] { theInitiatorId, theInitiatorId });
            depends.on<ICartRepository>().WhenToldTo(x => x.FindByUserGuid(new UserGuid(theInitiatorId.Id))).Return(theCart);
            command = new RemoveCartItem(theInitiatorId, theCartItemId);
        };

        private It should_tell_cart_to_remove_item = () => theCart.WasToldTo(x => x.RemoveItem(theCartItemId));
    }
}