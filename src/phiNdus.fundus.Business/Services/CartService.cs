using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Mails;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    public class CartService : BaseService
    {
        protected User User { get { return SecurityContext.SecuritySession.User; } }
        protected ICartRepository Carts { get { return GlobalContainer.Resolve<ICartRepository>(); } }

        public CartDto GetCart(int? version)
        {
            using (var uow = UnitOfWork.Start())
            {
                var cart = Carts.FindByCustomer(User);
                if (cart == null)
                {
                    cart = new Cart(User);
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
                var cart = Carts.FindByCustomer(User);
                if (cart == null)
                {
                    cart = new Cart(User);
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
                var cart = Carts.FindByCustomer(User);

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
                var cart = Carts.FindByCustomer(User);
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
                var cart = Carts.FindByCustomer(User);
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

        //public void CheckOut()
        //{
        //    using (var uow = UnitOfWork.Start())
        //    {
        //        var cart = FindCart();
        //        if (cart == null)
        //            throw new InvalidOperationException("Kein oder leerer Warenkorb");

        //        cart.Checkout();
        //        GlobalContainer.Resolve<IOrderRepository>().Save(cart);

        //        new OrderReceivedMail().For(cart)
        //            .Send(cart.Reserver)
        //            .Send(Settings.Common.AdminEmailAddress);

        //        uow.TransactionalFlush();
        //    }
        //}
        
    }
}