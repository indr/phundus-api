namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class UpdateCartItem : ICommand
    {
        public UpdateCartItem(CurrentUserId initiatorId, CurrentUserGuid initiatorGuid, Guid itemGuid, int quantity,
            DateTime fromUtc, DateTime toUtc)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            InitiatorId = initiatorId;
            InitiatorGuid = initiatorGuid;
            ItemGuid = itemGuid;
            Quantity = quantity;
            FromUtc = fromUtc;
            ToUtc = toUtc;
        }

        public CurrentUserId InitiatorId { get; protected set; }
        public CurrentUserGuid InitiatorGuid { get; protected set; }
        public Guid ItemGuid { get; protected set; }
        public int Quantity { get; protected set; }
        public DateTime FromUtc { get; protected set; }
        public DateTime ToUtc { get; protected set; }
    }

    public class UpdateCartItemHandler : IHandleCommand<UpdateCartItem>
    {
        private readonly ICartRepository _cartRepository;

        public UpdateCartItemHandler(ICartRepository cartRepository)
        {
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            _cartRepository = cartRepository;
        }

        public void Handle(UpdateCartItem command)
        {
            var cart = _cartRepository.FindByUserId(command.InitiatorId);
            if (cart == null)
                return;

            cart.UpdateItem(command.ItemGuid, command.Quantity, command.FromUtc, command.ToUtc);
        }
    }
}