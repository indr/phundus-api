namespace Phundus.Core.Shop.Orders.Commands
{
    using Cqrs;
    using Repositories;

    public class ClearCart
    {
        public int UserId { get; set; }
    }

    public class ClearCartHandler : IHandleCommand<ClearCart>
    {
        public ICartRepository CartRepository { get; set; }

        public void Handle(ClearCart command)
        {
            var cart = CartRepository.FindByCustomer(command.UserId);
            if (cart == null)
                return;

            cart.Clear();
        }
    }
}