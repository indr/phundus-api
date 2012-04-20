using System;
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
        public ActionResult Index()
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
            return Index();
        }
    }
}
