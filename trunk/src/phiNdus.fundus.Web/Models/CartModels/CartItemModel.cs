using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Models.CartModels
{
    public class CartItemModel : ViewModelBase
    {
        public CartItemModel()
        {
        }

        public CartItemModel(CartItemDto orderItemDto)
        {
            if (orderItemDto == null)
                throw new ArgumentNullException("orderItemDto");

            Id = orderItemDto.Id;
            Version = orderItemDto.Version;
            ArticleId = orderItemDto.ArticleId;
            Begin = orderItemDto.From;
            End = orderItemDto.To;
            Caption = orderItemDto.Text;
            Amount = orderItemDto.Quantity;
            UnitPrice = orderItemDto.UnitPrice;
            LineTotal = orderItemDto.LineTotal;
            //Availability = orderItemDto.Availability;

            if (Begin == DateTime.MinValue)
                Begin = SessionAdapter.ShopBegin;

            if (End == DateTime.MinValue)
                End = SessionAdapter.ShopEnd;
        }

        public void Update()
        {
            throw new NotImplementedException();
            //IoC.Resolve<ICartService>().AddItem(SessionId, CreateDto());
        }

        public OrderItemDto CreateDto()
        {
            var result = new OrderItemDto();
            result.Id = Id;
            result.Version = Version;
            result.Amount = Amount;
            result.ArticleId = ArticleId;
            result.OrderId = OrderId;
            result.From = Begin;
            result.To = End;
            return result;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int Version { get; set; }

        protected int OrderId { get; set; }
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
        public double UnitPrice { get; set; }

        [DisplayName("Preis")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double LineTotal { get; set; }

        public bool Availability { get; set; }
    }
}