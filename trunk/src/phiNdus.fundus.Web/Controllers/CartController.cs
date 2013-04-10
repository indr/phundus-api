namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Business;
    using Business.Assembler;
    using Business.Dto;
    using Business.SecuredServices;
    using Castle.Transactions;
    using Domain.Repositories;
    using Models;
    using Models.CartModels;
    using ViewModels;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class CartController : ControllerBase
    {
        protected static ICartService CartService
        {
            get { return GlobalContainer.Resolve<ICartService>(); }
        }

        public IUserRepository Users { get; set; }

        [Transaction]
        public virtual ActionResult Index(int? version)
        {
            var cartDto = CartService.GetCart(SessionId, version);

            var model = new CartModel();
            model.Load(cartDto);

            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Index(CartModel model)
        {
            var cartDto = CartService.GetCart(SessionId, model.Version);
            if (ModelState.IsValid)
                cartDto = CartService.UpdateCart(SessionId, model.CreateDto());
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

        [Transaction]
        public virtual ActionResult CheckOut()
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
            model.Customer = new UserModel(new UserAssembler().CreateDto(Users.Get(cartDto.CustomerId)));

            return View("CheckOut", model);
        }
    }
}