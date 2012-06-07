using System;
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

        public void Load(CartDto cartDto)
        {
            Id = cartDto.Id;
            Version = cartDto.Version;
            TotalPrice = cartDto.TotalPrice;
            Items.Clear();
            foreach (var each in cartDto.Items)
                Items.Add(new CartItemModel(each));
        }

        public void Save()
        {
            throw new NotImplementedException();
            var dtos = Items.Select(each => each.CreateDto()).ToList();
            // TODO: Ganzer Warenkorb updaten, damit Optimistic Offline Locking funktioniert
            //CartService.Update(cartDto);
            //CartService.UpdateItems(SessionId, dtos);
        }

        public IList<CartItemModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int Version { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double TotalPrice { get; set; }
    }
}