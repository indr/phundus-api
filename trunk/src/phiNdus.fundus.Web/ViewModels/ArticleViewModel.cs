namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Phundus.Core.Inventory.Queries;

    public class ArticleViewModel : ViewModelBase
    {
        private IList<ImageDto> _files = new List<ImageDto>();

        public ArticleViewModel()
        {
            Load(new ArticleDto());
        }

        public ArticleViewModel(ArticleDto articleDto)
        {
            Load(articleDto);
        }

        public int Id { get; set; }

        public int Version { get; set; }

        [DisplayName("Preis (inkl. MWSt)")]
        public double Price { get; set; }

        [DisplayName("Beschreibung")]
        public string Description { get; set; }

        [DisplayName("Spezifikation")]
        public string Specification { get; set; }


        public IList<ImageDto> Files
        {
            get { return _files; }
        }

        [DisplayName("Farbe")]
        public string Color { get; set; }

        [DisplayName("Bestand (Brutto)")]
        public int GrossStock { get; set; }

        [DisplayName("Marke")]
        public string Brand { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        private void Load(ArticleDto dto)
        {
            Id = dto.Id;
            Version = dto.Version;

            Name = dto.Name;
            Brand = dto.Brand;
            GrossStock = dto.GrossStock;
            Price =  Convert.ToDouble(dto.Price);
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
            result.Price = Convert.ToDecimal(Price);
            result.Description = Description;
            result.Specification = Specification;
            result.Color = Color;

            return result;
        }
    }
}