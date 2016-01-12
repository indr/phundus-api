namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class ClearCart : ICommand
    {
        public ClearCart(UserId initiatorId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
        }

        public UserId InitiatorId { get; protected set; }
    }

    public class ClearCartHandler : IHandleCommand<ClearCart>
    {
        private readonly ICartRepository _cartRepository;

        public ClearCartHandler(ICartRepository cartRepository)
        {
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            _cartRepository = cartRepository;
        }

        public void Handle(ClearCart command)
        {
            var cart = _cartRepository.FindByUserId(command.InitiatorId);
            if (cart == null)
                return;

            cart.Clear();
        }
    }
}