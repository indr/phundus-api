namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Helpers;
    using Iesi.Collections;
    using Microsoft.Practices.ServiceLocation;
    using Models.CartModels;
    using Phundus.Core.Inventory.Queries;
    using Phundus.Core.Inventory._Legacy.Dtos;
    using Phundus.Core.Inventory._Legacy.Services;

    public class ShopArticleViewModel : ArticleViewModel
    {
        private CartItemModel _cartItem = new CartItemModel();

        public ShopArticleViewModel(int id) : base(id)
        {
            CartItem.ArticleId = id;
            CartItem.Amount = 1;
            CartItem.Begin = SessionAdapter.ShopBegin;
            CartItem.End = SessionAdapter.ShopEnd;
            CanUserAddToCart = false;
        }

        public CartItemModel CartItem
        {
            get { return _cartItem; }
            set { _cartItem = value; }
        }

        public IList<AvailabilityDto> Availabilities { get; set; }
        public bool CanUserAddToCart { get; set; }
    }

    public class ArticleViewModel : ViewModelBase
    {
        
        private IList<ImageDto> _files = new List<ImageDto>();
        
        public ArticleViewModel()
        {
            Load(new ArticleDto());
        }

        public ArticleViewModel(int id)
        {
            var articleDto = ArticleQueries.GetArticle(id);
            
            Load(articleDto);
        }

        public ArticleViewModel(ArticleDto articleDto)
        {
            Load(articleDto);
        }

        protected IArticleQueries ArticleQueries
        {
            get { return ServiceLocator.Current.GetInstance<IArticleQueries>(); }
        }

        protected IArticleService ArticleService
        {
            get { return ServiceLocator.Current.GetInstance<IArticleService>(); }
        }

        public bool IsDeleted { get; set; }
        

       

        public int Id { get; set; }
        public int Version { get; set; }

       
        public string OrganizationName { get; set; }

        public int OrganizationId { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Specification
        
    {
        get;
        set;
    
        }

        
        public IList<ImageDto> Files
        {
            get { return _files; }
        }

        public IList<ImageDto> Images
        {
            get { return _files.Where(p => !p.FileName.EndsWith("pdf", true, CultureInfo.InvariantCulture)).ToList(); }
        }

        public IList<ImageDto> Documents
        {
            get { return _files.Where(p => p.FileName.EndsWith("pdf", true, CultureInfo.InvariantCulture)).ToList(); }
        }


        private void Load(ArticleDto article)
        {
            Id = article.Id;
            Version = article.Version;
            
            OrganizationId = article.OrganizationId;
            OrganizationName = article.OrganizationName;

            Name = article.Name;
            Brand = article.Brand;
            GrossStock = article.GrossStock;
            Price = article.Price;
            Description = article.Description;
            Specification = article.Specification;
            Color = article.Color;

            _files = article.Images;

        }

        public string Color { get; set; }

        public int GrossStock { get; set; }

        public string Brand { get; set; }

        public string Name { get; set; }

        public ArticleDto CreateDto()
        {
            var result = new ArticleDto();
            result.Id = Id;
            result.Version = Version;
            result.Name = Name;
            result.Brand = Brand;
            result.GrossStock = GrossStock;
            result.Price = Price;
            result.Description = Description;
            result.Specification = Specification;
            result.Color = Color;

            return result;
        }
    }
}