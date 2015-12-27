namespace Phundus.Core.Shop.Orders
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Common;
    using Cqrs;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Queries;
    using IdentityAndAccess.Users.Repositories;
    using Infrastructure;
    using Inventory.Services;
    using Mails;
    using Queries;
    using Repositories;
    using Services;
    using Shop.Services;

    public class CartService : ICartService
    {
        public ICartRepository Carts { get; set; }

        public IUserRepository Users { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberQueries MemberQueries { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        public ILessorService LessorService { get; set; }

        public IBorrowerService BorrowerService { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public IAvailabilityService AvailabilityService { get; set; }

        public CartDto GetCartByUserId(int userId)
        {
            var user = Users.FindById(userId);
            var cart = Carts.FindByCustomer(userId);
            if (cart == null)
            {
                cart = new Model.Cart(user);
                Carts.Add(cart);
            }

            cart.CalculateAvailability(AvailabilityService);
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

            cart = Carts.GetById(cart.Id);
            cart.CalculateAvailability(AvailabilityService);
            var assembler = new CartAssembler();
            return assembler.CreateDto(cart);
        }

        public CartDto UpdateCart(CartDto cartDto)
        {
            var assembler = new CartAssembler();
            var cart = assembler.CreateDomainObject(cartDto);

            Carts.Update(cart);

            cart.CalculateAvailability(AvailabilityService);

            return assembler.CreateDto(cart);
        }

        public CartDto RemoveItem(int userId, int itemId, int version)
        {
            var cart = Carts.FindByCustomer(userId);

            var item = cart.Items.SingleOrDefault(p => p.Id == itemId);
            if (item == null)
                throw new NotFoundException();
            if (item.Version != version)
                throw new DtoOutOfDateException();

            cart.RemoveItem(item);
            Carts.Update(cart);

            cart.CalculateAvailability(AvailabilityService);
            var assembler = new CartAssembler();
            return assembler.CreateDto(cart);
        }

        public ICollection<LegacyOrderDto> PlaceOrders(int userId)
        {
            var cart = Carts.FindByCustomer(userId);
            cart.CalculateAvailability(AvailabilityService);
            if (!cart.AreItemsAvailable)
                return null;

            var orders = cart.PlaceOrders(LessorService, BorrowerService, AvailabilityService);


            foreach (var order in orders)
            {
                var pdf = OrderPdfGeneratorService.GeneratePdf(order);
                var mail = new OrderReceivedMail().For(pdf, order);
                var chiefs = MemberQueries.ByOrganizationId(order.Organization.Id)
                    .Where(p => p.Role == (int) Role.Chief);

                foreach (var chief in chiefs)
                    mail.Send(chief.EmailAddress);
                
                // #34 Kein E-Mail mehr für den Ausleiher
                //mail.Send(order.Borrower.EmailAddress);
            }

            var assembler = new OrderDtoAssembler();
            return assembler.CreateDtos(orders);
        }
    }
}