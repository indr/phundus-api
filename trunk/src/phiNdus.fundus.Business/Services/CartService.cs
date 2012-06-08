using System;
using System.Linq;
using NHibernate;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    public class CartService : BaseService
    {
        protected User User { get { return SecurityContext.SecuritySession.User; } }

        public CartDto GetCart(int? version)
        {
            using (var uow = UnitOfWork.Start())
            {
                var carts = IoC.Resolve<ICartRepository>();
                var cart = carts.FindByCustomer(User);
                if (cart == null)
                {
                    cart = new Cart(User);
                    carts.Save(cart);
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
                var carts = IoC.Resolve<ICartRepository>();
                var cart = carts.FindByCustomer(User);
                cart.AddItem(item.ArticleId, item.Quantity, item.From, item.To);
                carts.SaveOrUpdate(cart);
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

                var carts = IoC.Resolve<ICartRepository>();
                carts.Update(cart);
                uow.TransactionalFlush();

                cart.CalculateAvailability();
                return assembler.CreateDto(cart);
            }
        }

        public CartDto RemoveItem(int id, int version)
        {
            using (var uow = UnitOfWork.Start())
            {
                var carts = IoC.Resolve<ICartRepository>();
                var cart = carts.FindByCustomer(User);

                var item = cart.Items.SingleOrDefault(p => p.Id == id);
                if (item == null)
                    throw new EntityNotFoundException();
                if (item.Version != version)
                    throw new DtoOutOfDateException();

                cart.RemoveItem(item);
                carts.Update(cart);
                uow.TransactionalFlush();

                cart.CalculateAvailability();
                var assembler = new CartAssembler();
                return assembler.CreateDto(cart);
            }
        }
    }
}