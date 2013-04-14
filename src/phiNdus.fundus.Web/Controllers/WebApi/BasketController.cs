namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System.Web;
    using System.Web.Http;
    using Castle.Transactions;
    using Domain.Repositories;

    public class BasketController : ApiController
    {
        public IUserRepository Users { get; set; }
        public ICartRepository Carts { get; set; }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Transaction]
        public virtual void Clear()
        {
            var user = Users.FindByEmail(User.Identity.Name);
            var cart = Carts.FindByCustomer(user);
            if (cart != null)
                cart.Clear();
        }
    }
}