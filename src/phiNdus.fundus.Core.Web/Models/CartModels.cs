using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace phiNdus.fundus.Core.Web.Models {

    public class CartModel {

        public CartModel() {
            this.Items = new List<CartItem>();
        }

        public List<CartItem> Items { get; set; }
    }

    /// <summary>
    /// Repräsentiert ein Item im Warenkorb.
    /// </summary>
    public class CartItem {
        public int ItemId { get; set; }

        [DisplayName("Bezeichnung")]
        public string Caption { get; set; }

        [Required]
        [DisplayName("Ausleihbeginn")]
        public DateTime Begin { get; set; }

        [Required]
        [DisplayName("Ausleihende")]
        public DateTime End { get; set; }

        [DisplayName("Anzahl")]
        public int Amount { get; set; }
    }
}