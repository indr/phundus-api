namespace phiNdus.fundus.Web.Business.Assembler
{
    using System.Linq;
    using Microsoft.Practices.ServiceLocation;
    using fundus.Business;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.Entities;
    using Phundus.Core.Repositories;

    public class CartAssembler
    {
        public CartDto CreateDto(Cart cart)
        {
            var result = new CartDto();
            result.Id = cart.Id;
            result.Version = cart.Version;
            result.CustomerId = cart.Customer.Id;
            result.AreItemsAvailable = cart.AreItemsAvailable;
            foreach (var each in cart.Items)
            {
                result.Items.Add(CreateItemDto(each));
            }
            return result;
        }

        private static CartItemDto CreateItemDto(CartItem each)
        {
            var result = new CartItemDto();
            result.Id = each.Id;
            result.Version = each.Version;
            result.Text = each.LineText;
            result.ArticleId = each.Article.Id;
            result.From = each.From;
            result.To = each.To;
            result.Quantity = each.Quantity;
            result.UnitPrice = each.UnitPrice;
            result.LineTotal = each.LineTotal;
            result.IsAvailable = each.IsAvailable;
            result.OrganizationName = each.Article.Organization.Name;

            return result;
        }

        public Cart CreateDomainObject(CartDto cartDto)
        {
            var carts = ServiceLocator.Current.GetInstance<ICartRepository>();
            var cart = carts.FindById(cartDto.Id);

            if (cart == null)
                throw new EntityNotFoundException();
            if (cart.Version != cartDto.Version)
                throw new DtoOutOfDateException();
            
            foreach (var itemDto in cartDto.Items)
            {
                var item = cart.Items.SingleOrDefault(p => p.Id == itemDto.Id);
                if (item == null)
                    throw new EntityNotFoundException();
                if (item.Version != itemDto.Version)
                    throw new DtoOutOfDateException();

                item.Quantity = itemDto.Quantity;
                item.From = itemDto.From;
                item.To = itemDto.To;
            }

            return cart;
        }
    }
}