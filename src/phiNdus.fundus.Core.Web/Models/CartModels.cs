using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace phiNdus.fundus.Core.Web.Models {

    // Todo,jac: create session-based-model klasse
    public class CartModel {

        //private static string SessionKey { get { return @"Cart"; } }

        //public CartModel(HttpSessionStateBase session) {
        //    this.Session = session;
        //    this.Load();
        //}

        //public List<string> Items { get; set; }

        //private HttpSessionStateBase Session { get; set; }

        //private void Load() {
        //    var items = this.Session[SessionKey] as List<string>;

        //    if (items == null) {
        //        items = new List<string>();
        //    }

        //    this.Items = items;
        //}

        //public void Persist() {
        //    this.Session[SessionKey] = this.Items;
        //}

        public CartModel() {
            this.Items = new List<string>();
        }

        public List<string> Items { get; set; }
    }
}