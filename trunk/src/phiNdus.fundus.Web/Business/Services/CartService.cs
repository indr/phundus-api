namespace phiNdus.fundus.Web.Business.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Assembler;
    using Dto;
    using fundus.Business;
    using Phundus.Core.IdentityAndAccess.Organizations.Model;
    using Phundus.Core.IdentityAndAccess.Queries;
    using Phundus.Core.IdentityAndAccess.Users.Repositories;
    using Phundus.Core.ReservationCtx.Mails;
    using Phundus.Core.ShopCtx;

    public class CartService : BaseService, ICartService
    {
        public ICartRepository Carts { get; set; }

        public IUserRepository Users { get; set; }

        public IMemberQueries MemberQueries { get; set; }

        #region ICartService Members

        public CartDto GetCart(int? version)
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user);
            if (cart == null)
            {
                cart = new Cart(user);
                Carts.Add(cart);
            }

            if (version.HasValue && cart.Version != version.Value)
                throw new DtoOutOfDateException();

            cart.CalculateAvailability(SessionFact());
            var assembler = new CartAssembler();
            return assembler.CreateDto(cart);
        }

        public CartDto AddItem(CartItemDto item)
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user);
            if (cart == null)
            {
                cart = new Cart(user);
                Carts.Add(cart);
            }
            cart.AddItem(item.ArticleId, item.Quantity, item.From, item.To);
            Carts.Add(cart);

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
            var cart = Carts.FindByCustomer(user);

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

        public OrderDto PlaceOrder()
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user);
            cart.CalculateAvailability(SessionFact());
            if (!cart.AreItemsAvailable)
                return null;

            var order = cart.PlaceOrder(SessionFact());

            var mail = new OrderReceivedMail().For(order);

            var chiefs = MemberQueries.ByOrganizationId(order.Organization.Id).Where(p => p.Role == (int)Role.Chief);
            
            foreach (var chief in chiefs)
                mail.Send(chief.EmailAddress);
            mail.Send(order.Reserver);


            var assembler = new OrderDtoAssembler();
            return assembler.CreateDto(order);
        }

        public ICollection<OrderDto> PlaceOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user);
            cart.CalculateAvailability(SessionFact());
            if (!cart.AreItemsAvailable)
                return null;

            var orders = cart.PlaceOrders(SessionFact());


            foreach (var order in orders)
            {
                var mail = new OrderReceivedMail().For(order);
                var chiefs = MemberQueries.ByOrganizationId(order.Organization.Id).Where(p => p.Role == (int)Role.Chief);

                foreach (var chief in chiefs)
                    mail.Send(chief.EmailAddress);
                mail.Send(order.Reserver);
            }

            var assembler = new OrderDtoAssembler();
            return assembler.CreateDtos(orders);
        }

        #endregion
    }
}