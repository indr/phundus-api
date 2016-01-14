namespace Phundus.Shop.Queries
{
    using Orders.Model;

    public class CartAssembler
    {
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
    }
}