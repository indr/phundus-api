namespace Phundus.Rest.Controllers
{
    using System.Web.Http;
    using System.Web.Security;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Users.Repositories;
    using Core.Shop.Orders.Repositories;

    public class BasketController : ApiControllerBase
    {
        public ICartRepository Carts { get; set; }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Transaction]
        public virtual void Clear()
        {
            var cart = Carts.FindByCustomer(CurrentUserId);
            if (cart != null)
                cart.Clear();
        }
    }
}