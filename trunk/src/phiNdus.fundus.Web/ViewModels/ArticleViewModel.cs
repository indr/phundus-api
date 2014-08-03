namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using Helpers;
    using Microsoft.Practices.ServiceLocation;
    using Models.CartModels;
    using Phundus.Core.Inventory.Queries;
    using Phundus.Core.Inventory._Legacy.Dtos;
    using Phundus.Core.Inventory._Legacy.Services;
    using Phundus.Core.Shop.Queries;

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

        public ArticleViewModel(ShopArticleSearchResultDto articleDto)
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

        [DisplayName("Preis (inkl.)")]
        public decimal Price { get; set; }

        [DisplayName("Beschreibung")]
        public string Description { get; set; }

        [DisplayName("Spezifikation")]
        public string Specification { get; set; }


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


        [DisplayName("Farbe")]
        public string Color { get; set; }

        [DisplayName("Bestand (Brutto)")]
        public int GrossStock { get; set; }

        [DisplayName("Marke")]
        public string Brand { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        private void Load(ShopArticleSearchResultDto dto)
        {
            Id = dto.Id;

            OrganizationName = dto.Organization;

            Name = dto.Name;
            Price = dto.Price;
            Description = dto.Description;
            Specification = dto.Specification;
        }

        private void Load(ArticleDto dto)
        {
            Id = dto.Id;
            Version = dto.Version;

            OrganizationId = dto.OrganizationId;
            OrganizationName = dto.OrganizationName;

            Name = dto.Name;
            Brand = dto.Brand;
            GrossStock = dto.GrossStock;
            Price = dto.Price;
            Description = dto.Description;
            Specification = dto.Specification;
            Color = dto.Color;

            _files = dto.Images;
        }

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