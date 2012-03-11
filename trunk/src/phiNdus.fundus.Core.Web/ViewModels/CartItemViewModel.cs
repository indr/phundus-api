using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
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

        private void Load(OrderItemDto orderItemDto, int articleId, double price)
        {
            ArticleId = articleId;
            Price = price;

            if (orderItemDto != null)
            {
                Id = orderItemDto.Id;
                Version = orderItemDto.Version;
                Begin = orderItemDto.From;
                End = orderItemDto.To;
            }
            
            if (Begin == DateTime.MinValue)
                Begin = SessionAdapter.ShopBegin;

            if(End == DateTime.MinValue)
                End = SessionAdapter.ShopEnd;
        }

        private OrderItemDto CreateDto()
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
        public int ArticleId { get; set; }

        [DisplayName("Bezeichnung")]
        public string Caption { get; set; }

        [DisplayName("Anzahl")]
        //[CustomValidation( <-- verfügbarkeit prüfen..
        public int Amount { get; set; }

        [Required]
        [DisplayName("Ausleihbeginn")]
        [DataType(DataType.Date)]
        //[CustomValidation( <-- verfügbarkeit prüfen..
        public DateTime Begin { get; set; }

        [Required]
        [DisplayName("Ausleihende")]
        [DataType(DataType.Date)]
        //[CustomValidation( <-- verfügbarkeit prüfen..
        public DateTime End { get; set; }

        [DisplayName("Preis")]
        public double Price { get; set; }

        public void Update()
        {
            IoC.Resolve<ICartService>().AddItem(SessionId, CreateDto());
        }
    }
}