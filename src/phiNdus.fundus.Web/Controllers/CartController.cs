using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Models.CartModels;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    public class CartController : ControllerBase
    {
        public ActionResult Index()
        {
            var model = new CartModel();
            model.Load();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CartModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Load();
                return View(model);
            }
            model.Save();
            return Index();
        }

        [HttpPost]
        public ActionResult Add(int articleId, DateTime from, DateTime to, int quantity)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Remove(int id, int version)
        {
            throw new NotImplementedException();

            // Hier nicht über ein Model gehen... 
            var service = IoC.Resolve<ICartService>();
            //service.RemoveItem(Session.SessionID, id, version);

            return Index();
        }


        public ActionResult CheckOut()
        {
            // Bestellen
            if (HttpContext.Request.HttpMethod == "POST")
            {
                var service = IoC.Resolve<IOrderService>();
                service.CheckOut(Session.SessionID);

                return View("CheckOutDone");
            }

            var model = new CheckOutViewModel();
            return View("CheckOut", model);
        }
    }
}