namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class RemoveCartItem : ICommand
    {
        public RemoveCartItem(UserId initiatorId, CartItemGuid cartItemGuid)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
            CartItemGuid = cartItemGuid;
        }

        public UserId InitiatorId { get; protected set; }
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
            var cart = _cartRepository.FindByUserId(command.InitiatorId);
            if (cart == null)
                return;

            cart.RemoveItem(command.CartItemGuid);
        }
    }
}