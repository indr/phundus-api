using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        private IList<CartItemViewModel> _items = new List<CartItemViewModel>();

        public CartViewModel()
        {
            Load(CartService.GetCart(SessionId));
        }

        private static ICartService CartService
        {
            get { return IoC.Resolve<ICartService>(); }
        }

        public IList<CartItemViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double TotalPrice { get; set; }

        private void Load(OrderDto orderDto)
        {
            TotalPrice = orderDto.TotalPrice;
            foreach (var each in orderDto.Items)
                Items.Add(new CartItemViewModel(each)); // TODO: Preis aufs DTO
        }

        public void Save()
        {
            var dtos = Items.Select(each => each.CreateDto()).ToList();
            CartService.UpdateItems(SessionId, dtos);
        }
    }
}