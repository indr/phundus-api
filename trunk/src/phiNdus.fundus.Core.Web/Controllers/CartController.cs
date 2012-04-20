using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.State;
using Rhino.Commons;
using phiNdus.fundus.Core.Web.ViewModels;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class CartController : ControllerBase
    {
        public ActionResult Index(IList<CartItemViewModel> items)
        {
            // Warenkorb aktualisieren
            if (HttpContext.Request.HttpMethod == "POST")
            {
                var model = new CartViewModel();
                foreach (var each in items)
                {
                    var item = model.Items.First(i => i.Id == each.Id);
                    item.Begin = each.Begin;
                    item.End = each.End;
                    item.Amount = each.Amount;

                }
                model.Save();
            }

            return CartOverview();
        }

        private ActionResult CartOverview()
        {
            var model = new CartViewModel();
            return View("Index", model);
        }

        /// <summary>
        /// Entfernt den "Artikel" aus dem Warenkorb
        /// </summary>
        /// <returns></returns>
        public ActionResult Remove(int id, int version)
        {
            // Hier nicht über ein Model gehen... 
            var service = IoC.Resolve<ICartService>();
            service.RemoveItem(Session.SessionID, id, version);

            // Warenkorb-Übersicht zurück geben
            return CartOverview();
        }
    }
}
