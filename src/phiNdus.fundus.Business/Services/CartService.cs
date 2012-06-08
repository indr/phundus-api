using System;
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

        public CartDto GetCart()
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

                var assembler = new CartAssembler();
                return assembler.CreateDto(cart);
            }
        }
    }
}