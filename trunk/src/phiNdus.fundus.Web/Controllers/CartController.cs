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
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(IList<CartItemModel> items)
        {
            var model = new CartModel();
            model.Load();
            foreach (var each in items)
            {
                var item = model.Items.First(i => i.Id == each.Id);
                item.Begin = each.Begin;
                item.End = each.End;
                item.Amount = each.Amount;
            }
            model.Save();

            return Index();
        }

        [HttpPost]
        public ActionResult Remove(int id, int version)
        {
            // Hier nicht über ein Model gehen... 
            var service = IoC.Resolve<ICartService>();
            service.RemoveItem(Session.SessionID, id, version);

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