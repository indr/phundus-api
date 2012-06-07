using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.Assembler
{
    public class CartAssembler
    {
        public CartDto CreateDto(Cart cart)
        {
            var result = new CartDto();
            result.Id = cart.Id;
            result.Version = cart.Version;
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

            result.ArticleId = each.Article.Id;
            result.From = each.From;
            result.To = each.To;
            result.Quantity = each.Quantity;

            // TODO: Properties für Preis und Bezeichnung (?)
            result.UnitPrice = each.Article.Price;
            result.Text = each.Article.Caption;

            result.LineTotal = each.LineTotal;

            return result;
        }
    }
}