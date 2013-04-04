namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using phiNdus.fundus.Domain.Repositories;

    public class HomeController : ControllerBase
    {
        public IUserRepository Users { get; set; }

        [Transaction]
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}