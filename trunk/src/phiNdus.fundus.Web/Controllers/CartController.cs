﻿namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using Models;
    using Models.CartModels;
    using Phundus.Core;
    using Phundus.Core.IdentityAndAccess.Queries;
    using Phundus.Core.Shop.Orders;
    using Phundus.Core.Shop.Queries;
    using Phundus.Web;
    using ViewModels;

    public class CartController : ControllerBase
    {
        public ICartService CartService { get; set; }

        public IUserQueries UserQueries { get; set; }

        [Transaction]
        public virtual ActionResult Index()
        {
            var cartDto = CartService.GetCart(CurrentUserId.Value);

            var model = new CartModel();
            model.Load(cartDto);

            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Index(CartModel model)
        {
            var cartDto = CartService.GetCart(model.Version);
            if (ModelState.IsValid)
                cartDto = CartService.UpdateCart(model.CreateDto());
            model.Load(cartDto);

            ModelState.Clear();
            return View(model);
        }

        [Transaction]
        public virtual ActionResult Remove(int id, int version)
        {
            CartDto cartDto;
            try
            {
                cartDto = CartService.RemoveItem(id, version);
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

        [Transaction]
        public virtual ActionResult CheckOut()
        {
            if (HttpContext.Request.HttpMethod == "POST")
            {
                var orderDtos = CartService.PlaceOrders();
                if (orderDtos != null)
                    return View("CheckOutDone");
                return RedirectToAction(CartActionNames.Index);
            }

            var cartDto = CartService.GetCart(CurrentUserId.Value);
            if (!cartDto.AreItemsAvailable)
                return RedirectToAction(CartActionNames.Index);

            var model = new CheckOutViewModel();
            model.Cart = new CartModel(cartDto);
            model.Customer = new UserModel(UserQueries.ById(cartDto.CustomerId));

            return View("CheckOut", model);
        }
    }
}