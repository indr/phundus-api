using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.Models;
using phiNdus.fundus.Core.Web.State;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{    
    public class CartController : Controller
    {
        private static class Actions {
            public static string List { get { return @"List"; } }
        }

        public CartController() {
            this.StateManager = IoC.Resolve<IStateManager>();
        }

        private IStateManager StateManager { get; set; }

        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return RedirectToAction(Actions.List);
        }

        public ActionResult List() {
            return View(this.StateManager.Load<CartModel>());
        }

        private static List<string> Captions = new List<string> {
            "Schwimmwesten (gelb)", "Kamera", "Schrauben", "Notizblöcke", "Funkgerät", "Pullover (grün)"
        };

        // mit post lösen
        public ActionResult Add() {
            var state = this.StateManager.Load<CartModel>();

            var random = new Random();

            state.Items.Add(new CartItem {
                Amount = random.Next(2, 34),
                Begin = DateTime.Now.AddDays(random.Next(0, 7)),
                End = DateTime.Now.AddDays(random.Next(8, 21)),
                ItemId = state.Items.Count() + 1,
                Caption = Captions[random.Next(0, Captions.Count)]
            });

            // ist eigentlich optional..
            this.StateManager.Save(state);

            return RedirectToAction(Actions.List);
        }

        public ActionResult Clear() {
            this.StateManager.Remove<CartModel>();

            return RedirectToAction(Actions.List);
        }
    }
}
