using System;
using System.Collections.Generic;
using System.Linq;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
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

        private void Load(OrderDto orderDto)
        {
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