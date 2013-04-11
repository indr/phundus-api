namespace phiNdus.fundus.Business.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Assembler;
    using Domain.Entities;
    using Domain.Repositories;
    using Dto;
    using Mails;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class CartService : BaseService, ICartService
    {
        public ICartRepository Carts { get; set; }
        public IUserRepository Users { get; set; }

        #region ICartService Members

        public CartDto GetCart(int? version)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var cart = Carts.FindByCustomer(user);
                if (cart == null)
                {
                    cart = new Cart(user);
                    Carts.Save(cart);
                    uow.TransactionalFlush();
                }

                if (version.HasValue && cart.Version != version.Value)
                    throw new DtoOutOfDateException();

                cart.CalculateAvailability();
                var assembler = new CartAssembler();
                return assembler.CreateDto(cart);
            }
        }

        public CartDto AddItem(CartItemDto item)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var cart = Carts.FindByCustomer(user);
                if (cart == null)
                {
                    cart = new Cart(user);
                    Carts.Save(cart);
                }
                cart.AddItem(item.ArticleId, item.Quantity, item.From, item.To);
                Carts.SaveOrUpdate(cart);
                uow.TransactionalFlush();

                cart.CalculateAvailability();
                var assembler = new CartAssembler();
                return assembler.CreateDto(cart);
            }
        }

        public CartDto UpdateCart(CartDto cartDto)
        {
            using (var uow = UnitOfWork.Start())
            {
                var assembler = new CartAssembler();
                var cart = assembler.CreateDomainObject(cartDto);

                Carts.Update(cart);
                uow.TransactionalFlush();

                cart.CalculateAvailability();
                return assembler.CreateDto(cart);
            }
        }

        public CartDto RemoveItem(int id, int version)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var cart = Carts.FindByCustomer(user);

                var item = cart.Items.SingleOrDefault(p => p.Id == id);
                if (item == null)
                    throw new EntityNotFoundException();
                if (item.Version != version)
                    throw new DtoOutOfDateException();

                cart.RemoveItem(item);
                Carts.Update(cart);
                uow.TransactionalFlush();

                cart.CalculateAvailability();
                var assembler = new CartAssembler();
                return assembler.CreateDto(cart);
            }
        }

        public OrderDto PlaceOrder()
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var cart = Carts.FindByCustomer(user);
                cart.CalculateAvailability();
                if (!cart.AreItemsAvailable)
                    return null;

                var order = cart.PlaceOrder();

                var mail = new OrderReceivedMail().For(order);
                var chiefs = order.Organization.Memberships.Where(m => m.Role == Role.Chief.Id);
                foreach (var chief in chiefs)
                    mail.Send(chief.User.Membership.Email);
                mail.Send(order.Reserver);

                uow.TransactionalFlush();

                var assembler = new OrderDtoAssembler();
                return assembler.CreateDto(order);
            }
        }

        public ICollection<OrderDto> PlaceOrders()
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var cart = Carts.FindByCustomer(user);
                cart.CalculateAvailability();
                if (!cart.AreItemsAvailable)
                    return null;

                var orders = cart.PlaceOrders();

                uow.TransactionalFlush();

                foreach (var order in orders)
                {
                    var mail = new OrderReceivedMail().For(order);
                    var chiefs = order.Organization.Memberships.Where(m => m.Role == Role.Chief.Id);
                    foreach (var chief in chiefs)
                        mail.Send(chief.User.Membership.Email);
                    mail.Send(order.Reserver);
                }

                var assembler = new OrderDtoAssembler();
                return assembler.CreateDtos(orders);
            }
        }

        #endregion
    }
}