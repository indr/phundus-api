namespace Phundus.Core.Shop.Application.Commands
{
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Ordering;

    public class ClearCart : ICommand
    {
        public int InitiatorId { get; set; }
    }

    public class ClearCartHandler : IHandleCommand<ClearCart>
    {
        public ICartRepository CartRepository { get; set; }

        public void Handle(ClearCart command)
        {
            var cart = CartRepository.FindByCustomer(command.InitiatorId);
            if (cart == null)
                return;

            cart.Clear();
        }
    }
}