using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.Models;
using phiNdus.fundus.Core.Web.State;
using Rhino.Commons;
using phiNdus.fundus.Core.Web.ViewModels;
using System.Globalization;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class CartController : ControllerBase
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

        //
        // GET: /Cart/List

        public ActionResult List() {
            return View(this.StateManager.Load<CartModel>());
        }

        //
        // GET: /Cart/Clear

        public ActionResult Clear() {
            this.StateManager.Remove<CartModel>();

            return RedirectToAction(Actions.List);
        }

        //
        // POST: /Cart/AddItem

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddItem(CartItem cartItem) {
            // Todo,jac: Verfügubarkeit prüfen

            // Todo,jac: Preis ermitteln

            // Todo,jac: Caption aus DB laden
            cartItem.Caption = string.Format("[Id={0}]", cartItem.ItemId);
            cartItem.Gid = Guid.NewGuid();

            var state = this.StateManager.Load<CartModel>();

            var response = this.AddCartItemInternal(state, cartItem);

            this.StateManager.Save(state);

            if (Request.IsAjaxRequest()) {
                return DisplayFor(response);
            } else {
               return RedirectToAction(Actions.List);
            }
        }

        //
        // GET: /Cart/RemoveItem/Id

        public ActionResult RemoveItem(Guid gid) {
            var state = this.StateManager.Load<CartModel>();

            state.Items.RemoveAll(i => i.Gid == gid);

            this.StateManager.Save(state);

            return RedirectToAction(Actions.List);
        }

        //
        // GET: /Cart/GetCartItemsCount

        [AcceptVerbs(HttpVerbs.Get)]
        public int GetCartItemsCount() {
            return this.StateManager.Load<CartModel>().Items.Count;
        }

        private bool DateIsEqual(DateTime x, DateTime y) {
            return x.Year == y.Year && x.Month == y.Month && x.Day == y.Day;
        }

        private MessageBoxViewModel AddCartItemInternal(CartModel state, CartItem newItem) {
            MessageBoxViewModel response;

            if (newItem.Begin >= newItem.End)
            {
                response = new MessageBoxViewModel
                {
                    Type = MessageBoxType.Error,
                    Message = "Das Anfangsdatum muss vor dem Enddatum liegen."
                };
            }
            else
            {
                var existingItem = state.Items.SingleOrDefault(s =>
                    s.ItemId == newItem.ItemId &&
                    DateIsEqual(s.Begin, newItem.Begin) &&
                    DateIsEqual(s.End, newItem.End));

                if (existingItem != null)
                {
                    existingItem.Amount += newItem.Amount;
                    response = new MessageBoxViewModel
                    {
                        Type = MessageBoxType.Warning,
                        Message = string.Format(CultureInfo.InvariantCulture,
                            "Dieser Artikel war für den angegebenen Ausleihzeitraum bereits im Warenkorb. Die Anzahl dieses Artikels im Warenkorb wurde um {0} auf {1} erhöht.", newItem.Amount, existingItem.Amount)
                    };
                }
                else
                {
                    state.Items.Add(newItem);

                    response = new MessageBoxViewModel
                    {
                        Type = MessageBoxType.Success,
                        Message = "Der Artikel wurde dem Warenkorb hinzugefügt."
                    };
                }
            }
            return response;
        }
    }
}
