namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

    public class RemoveCartItem : ICommand
    {
        public RemoveCartItem(InitiatorId initiatorId, CartItemId cartItemId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
            CartItemId = cartItemId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public CartItemId CartItemId { get; protected set; }
    }

    public class RemoveCartItemHandler : IHandleCommand<RemoveCartItem>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveCartItemHandler(ICartRepository cartRepository)
        {
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            _cartRepository = cartRepository;
        }

        [Transaction]
        public void Handle(RemoveCartItem command)
        {
            var cart = _cartRepository.FindByUserGuid(new UserId(command.InitiatorId.Id));
            if (cart == null)
                return;

            cart.RemoveItem(command.CartItemId);
        }
    }
}