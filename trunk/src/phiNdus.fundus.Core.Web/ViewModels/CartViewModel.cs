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
                Items.Add(new CartItemViewModel(each, each.ArticleId, 0.99)); // TODO: Preis aufs DTO
        }
    }


    
}