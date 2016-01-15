using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using phiNdus.fundus.Web.Helpers;
using phiNdus.fundus.Web.ViewModels;

namespace phiNdus.fundus.Web.Models.CartModels
{
    using Phundus.Shop.Queries;
    using Phundus.Web.Models.CartModels;

    public class CartItemModel : ViewModelBase
    {
        public CartItemModel(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        public Guid UserGuid { get; private set; }

        public string OrganizationName { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public int Version { get; set; }

        public int ArticleId { get; set; }

        [DisplayName("Bezeichnung")]
        public string Caption { get; set; }

        [Min(1)]
        [DisplayName("Anzahl")]
        public int Amount { get; set; }

        [Required]
        [DisplayName("Ausleihbeginn")]
        [DataType(DataType.Date)]
        public DateTime Begin { get; set; }

        [Required]
        [DisplayName("Ausleihende")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [DisplayName("Einzelpreis")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Preis")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal LineTotal { get; set; }

        public bool IsAvailable { get; set; }

        public CartItemDto CreateDto()
        {
            var result = new CartItemDto();
            result.Id = Id;
            result.Version = Version;
            result.ArticleId = ArticleId;
            result.Quantity = Amount;
            result.From = Begin;
            result.To = End;
            return result;
        }
    }
}