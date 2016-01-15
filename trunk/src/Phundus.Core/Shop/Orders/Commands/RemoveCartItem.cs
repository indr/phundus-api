namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class RemoveCartItem : ICommand
    {
        public RemoveCartItem(InitiatorGuid initiatorId, CartItemGuid cartItemGuid)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorGuid = initiatorId;
            CartItemGuid = cartItemGuid;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
        public CartItemGuid CartItemGuid { get; protected set; }
    }

    public class RemoveCartItemHandler : IHandleCommand<RemoveCartItem>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveCartItemHandler(ICartRepository cartRepository)
        {
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            _cartRepository = cartRepository;
        }

        public void Handle(RemoveCartItem command)
        {
            var cart = _cartRepository.FindByUserGuid(new UserGuid(command.InitiatorGuid.Id));
            if (cart == null)
                return;

            cart.RemoveItem(command.CartItemGuid);
        }
    }
}