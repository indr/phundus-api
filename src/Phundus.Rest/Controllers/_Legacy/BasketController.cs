namespace Phundus.Rest.Controllers
{
    using System.Web.Http;
    using System.Web.Security;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Users.Repositories;
    using Core.Shop.Orders.Repositories;

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
            var cart = Carts.FindByCustomer(user.Id);
            if (cart != null)
                cart.Clear();
        }
    }
}