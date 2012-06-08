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
        protected static ICartService CartService
        {
            get { return IoC.Resolve<ICartService>(); }
        }

        public ActionResult Index(int? version)
        {
            var cartDto = CartService.GetCart(SessionId, version);
            
            var model = new CartModel();
            model.Load(cartDto);

            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CartModel model)
        {
            var cartDto = CartService.GetCart(SessionId, model.Version);
            if (ModelState.IsValid)
                cartDto = CartService.UpdateCart(SessionId, model.CreateDto());
            model.Load(cartDto);

            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult Remove(int id, int version)
        {
            

            throw new NotImplementedException();

            // Hier nicht über ein Model gehen... 
            var service = IoC.Resolve<ICartService>();
            //service.RemoveItem(Session.SessionID, id, version);

            //return Index();
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