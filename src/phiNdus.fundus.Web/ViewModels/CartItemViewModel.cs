using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public static class SessionAdapter
    {
        public static DateTime ShopBegin
        {
            get { return DateTime.Today.AddDays(1); }
        }

        public static DateTime ShopEnd
        {
            get { return DateTime.Today.AddDays(7); }
        }
    }

    public class CartItemViewModel : ViewModelBase
    {
        public CartItemViewModel()
        {
            
        }

        public CartItemViewModel(OrderItemDto orderItemDto, int articleId, double price)
        {
            Load(orderItemDto, articleId, price);
        }

        public CartItemViewModel(OrderItemDto orderItemDto)
        {
            Load(orderItemDto, orderItemDto.ArticleId, 0.99);
        }

        private void Load(OrderItemDto orderItemDto, int articleId, double price)
        {
            ArticleId = articleId;
            LineTotal = price;

            if (orderItemDto != null)
            {
                Id = orderItemDto.Id;
                Version = orderItemDto.Version;
                Begin = orderItemDto.From;
                End = orderItemDto.To;
                Caption = orderItemDto.Text;
                Amount = orderItemDto.Amount;
                UnitPrice = orderItemDto.UnitPrice;
                LineTotal = orderItemDto.LineTotal;
                Availability = orderItemDto.Availability;
            }
            
            if (Begin == DateTime.MinValue)
                Begin = SessionAdapter.ShopBegin;

            if(End == DateTime.MinValue)
                End = SessionAdapter.ShopEnd;
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

        public int Id { get; set; }
        public int Version { get; set; }
        protected int OrderId { get; set; }

        [Required]
        [Min(1)]
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

        public void Update()
        {
            IoC.Resolve<ICartService>().AddItem(SessionId, CreateDto());
        }
    }
}