namespace Phundus.Core.Shop.Orders
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Contracts.Services;
    using Cqrs;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Queries;
    using IdentityAndAccess.Users.Repositories;
    using Infrastructure;
    using Mails;
    using Queries;
    using Repositories;

    public class CartService : AppServiceBase, ICartService
    {
        public ICartRepository Carts { get; set; }

        public IUserRepository Users { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberQueries MemberQueries { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        public IBorrowerService BorrowerService { get; set; }

        public CartDto GetCartByUserId(int userId)
        {
            var user = Users.FindById(userId);
            var cart = Carts.FindByCustomer(userId);
            if (cart == null)
            {
                cart = new Model.Cart(user);
                Carts.Add(cart);
            }

            cart.CalculateAvailability(SessionFact());
            var assembler = new CartAssembler();
            return assembler.CreateDto(cart);
        }

        public CartDto AddItem(int? cartId, int userId, CartItemDto item)
        {
            Model.Cart cart = null;
            if (cartId.HasValue)
                cart = Carts.FindById(cartId.Value);

            if (cart == null)
            {
                var user = Users.FindById(userId);
                cart = new Model.Cart(user);
                Carts.Add(cart);
            }
            cartId = cart.Id;

            Dispatcher.Dispatch(new AddArticleToCart
            {
                ArticleId = item.ArticleId,
                CartId = cartId.Value,
                DateFrom = item.From,
                DateTo = item.To,
                Quantity = item.Quantity,
                InitiatorId = userId
            });

            cart = Carts.ById(cart.Id);
            cart.CalculateAvailability(SessionFact());
            var assembler = new CartAssembler();
            return assembler.CreateDto(cart);
        }

        public CartDto UpdateCart(CartDto cartDto)
        {
            var assembler = new CartAssembler();
            var cart = assembler.CreateDomainObject(cartDto);

            Carts.Update(cart);


            cart.CalculateAvailability(SessionFact());
            return assembler.CreateDto(cart);
        }

        public CartDto RemoveItem(int id, int version)
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user.Id);

            var item = cart.Items.SingleOrDefault(p => p.Id == id);
            if (item == null)
                throw new EntityNotFoundException();
            if (item.Version != version)
                throw new DtoOutOfDateException();

            cart.RemoveItem(item);
            Carts.Update(cart);


            cart.CalculateAvailability(SessionFact());
            var assembler = new CartAssembler();
            return assembler.CreateDto(cart);
        }

        public ICollection<OrderDto> PlaceOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user.Id);
            cart.CalculateAvailability(SessionFact());
            if (!cart.AreItemsAvailable)
                return null;

            var orders = cart.PlaceOrders(SessionFact(), BorrowerService);


            foreach (var order in orders)
            {
                var mail = new OrderReceivedMail().For(order, OrganizationRepository.ById(order.OrganizationId));
                var chiefs = MemberQueries.ByOrganizationId(order.OrganizationId)
                    .Where(p => p.Role == (int) Role.Chief);

                foreach (var chief in chiefs)
                    mail.Send(chief.EmailAddress);
                mail.Send(order.Borrower.EmailAddress);
            }

            var assembler = new OrderDtoAssembler();
            return assembler.CreateDtos(orders);
        }
    }
}