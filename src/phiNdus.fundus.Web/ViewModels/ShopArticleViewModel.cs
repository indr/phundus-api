namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Bootstrap;
    using Helpers;
    using Models.CartModels;
    using Phundus.Inventory.Queries;
    using Phundus.Shop.Queries;

    public class ShopArticleViewModel
    {
        private CartItemModel _cartItem;

        public ShopArticleViewModel(ShopArticleDetailDto dto, Guid userGuid)
        {
            _cartItem = new CartItemModel(userGuid);
            CartItem.ArticleId = dto.Id;
            CartItem.Amount = 1;
            CartItem.Begin = SessionAdapter.ShopBegin;
            CartItem.End = SessionAdapter.ShopEnd;
            CanUserAddToCart = false;

            Article = dto;
            Article.Images.ForEach(image => image.FileName = GenerateImageFileName(Article.Id, image));
        }

        private string GenerateImageFileName(int articleId, ShopArticleImageDto image)
        {
            return String.Format(@"~\Content\Images\Articles\{0}\{1}", articleId, image.FileName);
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