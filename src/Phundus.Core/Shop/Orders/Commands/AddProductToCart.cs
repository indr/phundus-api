namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using System.Security;
    using Cqrs;
    using Repositories;

    public class AddProductToCart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }

        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class AddProductToCartHandler : IHandleCommand<AddProductToCart>
    {
        public ICartRepository CartRepository { get; set; }

        public void Handle(AddProductToCart command)
        {
            var cart = CartRepository.FindById(command.CartId);
            if (cart == null)
                throw new CartNotFoundException();

            if (cart.CustomerId != command.UserId)
                throw new SecurityException();

            cart.AddItem(command.ArticleId, command.Quantity,
                command.DateFrom, command.DateTo);
        }
    }
}