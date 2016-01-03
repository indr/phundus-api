namespace Phundus.Core.Shop.Orders
{
    using System.Linq;
    using Commands;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAndAccess.Users.Repositories;
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

        public ICommandDispatcher Dispatcher { get; set; }

        public ILessorService LessorService { get; set; }

        public ILesseeService LesseeService { get; set; }

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
                InitiatorId = new UserId(userId)
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

        public bool PlaceOrders(int userId)
        {
            var cart = Carts.FindByCustomer(userId);
            cart.CalculateAvailability(AvailabilityService);
            if (!cart.AreItemsAvailable)
                return false;

            var orders = cart.PlaceOrders(LessorService, LesseeService, AvailabilityService);


            foreach (var order in orders)
            {
                var pdf = OrderPdfGeneratorService.GeneratePdf(order);
                var mail = new OrderReceivedMail().For(pdf, order);
                var managers = LessorService.GetManagers(order.Lessor.LessorId.Id);

                foreach (var manager in managers)
                    mail.Send(manager.EmailAddress);

                // #34 Kein E-Mail mehr für den Ausleiher
                //mail.Send(order.Borrower.EmailAddress);
            }

            return orders.Count > 0;
        }
    }
}