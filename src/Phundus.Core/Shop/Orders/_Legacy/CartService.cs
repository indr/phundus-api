namespace Phundus.Shop.Orders
{
    using Commands;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Users.Repositories;
    using Inventory.Services;
    using Queries;
    using Repositories;

    public class CartService : ICartService
    {
        public ICartRepository Carts { get; set; }

        public IUserRepository Users { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        public IAvailabilityService AvailabilityService { get; set; }

        public CartDto GetCartByUserId(int userId)
        {
            var user = Users.FindById(userId);
            var cart = Carts.FindByUserId(new UserId(user.Id));
            if (cart == null)
            {
                cart = new Model.Cart(new UserId(user.Id), new UserGuid(user.Guid));
                Carts.Add(cart);
            }

            cart.CalculateAvailability(AvailabilityService);
            var assembler = new CartAssembler(Carts);
            return assembler.CreateDto(cart);
        }

        public CartDto AddItem(int? cartId, int userId, CartItemDto item)
        {
            Model.Cart cart = null;
            if (cartId.HasValue)
                cart = Carts.FindById(cartId.Value);

            var user = Users.FindById(userId);
            if (cart == null)
            {
                cart = new Model.Cart(new UserId(user.Id), new UserGuid(user.Guid));
                Carts.Add(cart);
            }

            Dispatcher.Dispatch(new AddArticleToCart(new UserId(userId), new UserGuid(user.Guid),
                new ArticleId(item.ArticleId), item.From,
                item.To, item.Quantity));

            cart = Carts.GetById(cart.Id);
            cart.CalculateAvailability(AvailabilityService);
            var assembler = new CartAssembler(Carts);
            return assembler.CreateDto(cart);
        }
    }
}