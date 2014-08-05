namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using Helpers;
    using Models.CartModels;
    using Phundus.Core.Inventory.Queries;
    using Phundus.Core.Shop.Queries;

    public class ShopArticleViewModel
    {
        private CartItemModel _cartItem = new CartItemModel();

        public ShopArticleViewModel(ShopArticleDetailDto dto)
        {
            CartItem.ArticleId = dto.Id;
            CartItem.Amount = 1;
            CartItem.Begin = SessionAdapter.ShopBegin;
            CartItem.End = SessionAdapter.ShopEnd;
            CanUserAddToCart = false;

            Article = dto;
        }

        public ShopArticleDetailDto Article { get; set; }

        public CartItemModel CartItem
        {
            get { return _cartItem; }
            set { _cartItem = value; }
        }

        public IList<AvailabilityDto> Availabilities { get; set; }

        public bool CanUserAddToCart { get; set; }
    }
}