using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using phiNdus.fundus.Web.ViewModels;

namespace phiNdus.fundus.Web.Models.CartModels
{
    using System.Linq;
    using Business.Dto;

    public class CartModel : ViewModelBase
    {
        private IList<CartItemModel> _items = new List<CartItemModel>();

        public CartModel()
        {
            
        }

        public CartModel(CartDto cartDto)
        {
            Load(cartDto);
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

        public void Load(CartDto cartDto)
        {
            Id = cartDto.Id;
            Version = cartDto.Version;
            TotalPrice = cartDto.TotalPrice;
            Items.Clear();
            foreach (var each in cartDto.Items.OrderBy(k => k.OrganizationName))
                Items.Add(new CartItemModel(each));
        }

        public CartDto CreateDto()
        {
            var result = new CartDto();
            result.Id = Id;
            result.Version = Version;
            foreach (var each in Items)
                result.Items.Add(each.CreateDto());
            return result;
        }
    }
}