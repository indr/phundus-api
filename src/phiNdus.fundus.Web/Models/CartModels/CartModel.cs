using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Models.CartModels
{
    public class CartModel : ViewModelBase
    {
        private IList<CartItemModel> _items = new List<CartItemModel>();

        private static ICartService CartService
        {
            get { return IoC.Resolve<ICartService>(); }
        }

        public void Load()
        {
            var cartDto = CartService.GetCart(SessionId);
            Load(cartDto);
        }

        public void Load(OrderDto cartDto)
        {
            TotalPrice = cartDto.TotalPrice;
            foreach (var each in cartDto.Items)
                Items.Add(new CartItemModel(each));
        }

        public void Save()
        {
            var dtos = Items.Select(each => each.CreateDto()).ToList();
            CartService.UpdateItems(SessionId, dtos);
        }

        public IList<CartItemModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double TotalPrice { get; set; }
    }
}