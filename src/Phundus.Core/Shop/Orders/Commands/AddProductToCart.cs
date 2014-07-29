namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Cqrs;
    using IdentityAndAccess.Queries;
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

            cart.AddItem(command.ArticleId, command.Quantity,
                command.DateFrom, command.DateTo);
        }
    }

    public class CartNotFoundException : Exception
    {
    }
}