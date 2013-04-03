using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Business;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Models;
using phiNdus.fundus.Web.Models.CartModels;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    public class CartController : ControllerBase
    {
        protected static ICartService CartService
        {
            get { return GlobalContainer.Resolve<ICartService>(); }
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

        public ActionResult Remove(int id, int version)
        {
            CartDto cartDto;
            try
            {
                cartDto = CartService.RemoveItem(SessionId, id, version);
            }
            // Nicht besonders RESTful...
            catch (EntityNotFoundException)
            {
                // TODO: Meldung anzeigen, dass das Item nicht gefunden werden konnte, evtl. weil der
                // Warenkorb in der Zwischenzeit bereits verändert wurde...
                return RedirectToAction(CartActionNames.Index);
            }
            // Nicht besonders RESTful...
            catch (DtoOutOfDateException)
            {
                // TODO: Meldung anzeigen, dass das Item nicht entfernt werden konnte, da der Warenkorb
                // in der Zwischenzeit bereits verändert wurde...
                return RedirectToAction(CartActionNames.Index);
            }

            var model = new CartModel();
            model.Load(cartDto);

            ModelState.Clear();
            return View("Index", model);
        }

        public ActionResult CheckOut()
        {
            if (HttpContext.Request.HttpMethod == "POST")
            {
                var orderDtos = CartService.PlaceOrders(Session.SessionID);
                if (orderDtos != null)
                    return View("CheckOutDone");
                return RedirectToAction(CartActionNames.Index);
            }

            var cartDto = CartService.GetCart(SessionId, null);
            if (!cartDto.AreItemsAvailable)
                return RedirectToAction(CartActionNames.Index);

            var model = new CheckOutViewModel();
            model.Cart = new CartModel(cartDto);
            model.Customer = new UserModel(cartDto.CustomerId);

            return View("CheckOut", model);
        }
    }
}