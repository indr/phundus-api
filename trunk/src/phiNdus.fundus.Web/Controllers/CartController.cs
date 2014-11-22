namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Application;
    using Core.Shop.Orders;
    using Core.Shop.Queries;
    using Infrastructure;
    using phiNdus.fundus.Web.Models;
    using phiNdus.fundus.Web.Models.CartModels;
    using phiNdus.fundus.Web.ViewModels;

    public class CartController : ControllerBase
    {
        public ICartService CartService { get; set; }

        public IUserQueries UserQueries { get; set; }

        [Transaction]
        public virtual ActionResult Index()
        {
            var cartDto = CartService.GetCartByUserId(CurrentUserId);

            var model = new CartModel();
            model.Load(cartDto);

            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Index(CartModel model)
        {
            var cartDto = CartService.GetCartByUserId(CurrentUserId);
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
                cartDto = CartService.RemoveItem(CurrentUserId, id, version);
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
                var orderDtos = CartService.PlaceOrders(CurrentUserId);
                if (orderDtos != null)
                    return View("CheckOutDone");
                return RedirectToAction(CartActionNames.Index);
            }

            var cartDto = CartService.GetCartByUserId(CurrentUserId);
            if (!cartDto.AreItemsAvailable)
                return RedirectToAction(CartActionNames.Index);

            var model = new CheckOutViewModel();
            model.Cart = new CartModel(cartDto);
            model.Customer = new UserModel(UserQueries.ById(cartDto.CustomerId));

            return View("CheckOut", model);
        }
    }
}