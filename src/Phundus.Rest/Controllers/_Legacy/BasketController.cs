namespace Phundus.Rest.Controllers
{
    using System.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccessCtx.Repositories;
    using Core.ShopCtx;

    public class BasketController : ApiControllerBase
    {
        public IUserRepository Users { get; set; }
        public ICartRepository Carts { get; set; }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Transaction]
        public virtual void Clear()
        {
            var user = Users.FindByEmail(Identity.Name);
            var cart = Carts.FindByCustomer(user);
            if (cart != null)
                cart.Clear();
        }
    }
}