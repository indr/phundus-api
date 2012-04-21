using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace phiNdus.fundus.Web.Models {

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

        public Guid Gid { get; set; }

        public int ItemId { get; set; }

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