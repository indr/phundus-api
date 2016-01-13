namespace Phundus.Core.Shop.Queries
{
    using System.Linq;
    using Common;
    using Orders.Model;
    using Orders.Repositories;

    public class CartAssembler
    {
        private ICartRepository _cartRepository;

        public CartAssembler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public CartDto CreateDto(Cart cart)
        {
            var result = new CartDto();
            result.Id = cart.Id;
            result.Version = cart.Version;
            result.CustomerId = cart.CustomerId;
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
            result.ArticleId = each.Article.ArticleId;
            result.From = each.From;
            result.To = each.To;
            result.Quantity = each.Quantity;
            result.UnitPrice = each.UnitPrice;
            result.LineTotal = each.ItemTotal;
            result.IsAvailable = each.IsAvailable;
            result.OrganizationName = each.Article.Owner.Name;

            return result;
        }

        public Cart CreateDomainObject(CartDto cartDto)
        {
            var cart = _cartRepository.FindById(cartDto.Id);

            if (cart == null)
                throw new NotFoundException();

            foreach (var itemDto in cartDto.Items)
            {
                var item = cart.Items.SingleOrDefault(p => p.Id == itemDto.Id);
                if (item == null)
                    throw new NotFoundException();

                item.Quantity = itemDto.Quantity;
                item.From = itemDto.From;
                item.To = itemDto.To;
            }

            return cart;
        }
    }
}