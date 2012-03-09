using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        public CartViewModel()
        {
            Load(CartService.GetCart(SessionId));
        }

        private static ICartService CartService
        {
            get { return IoC.Resolve<ICartService>(); }
        }

        public IList<CartItemViewModel> Items { get; set; }

        private void Load(OrderDto orderDto)
        {
            foreach (var each in orderDto.Items)
                Items.Add(new CartItemViewModel(each));
        }
    }


    public class CartItemViewModel : ViewModelBase
    {
        public CartItemViewModel(OrderItemDto orderItemDto)
        {
            Load(orderItemDto);
        }

        private void Load(OrderItemDto orderItemDto)
        {
            Caption = orderItemDto.ArticleId.ToString();
        }

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
        public decimal Price { get; set; }
    }
}