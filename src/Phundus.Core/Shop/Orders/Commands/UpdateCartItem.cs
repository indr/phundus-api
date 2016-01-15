namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class UpdateCartItem : ICommand
    {
        public UpdateCartItem(InitiatorGuid initiatorGuid, Guid itemGuid, int quantity,
            DateTime fromUtc, DateTime toUtc)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");            
            InitiatorGuid = initiatorGuid;
            ItemGuid = itemGuid;
            Quantity = quantity;
            FromUtc = fromUtc;
            ToUtc = toUtc;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
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
            var cart = _cartRepository.FindByUserGuid(new UserGuid(command.InitiatorGuid.Id));
            if (cart == null)
                return;

            cart.UpdateItem(command.ItemGuid, command.Quantity, command.FromUtc, command.ToUtc);
        }
    }
}