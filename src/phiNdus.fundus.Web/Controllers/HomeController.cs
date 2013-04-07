namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : ControllerBase
    {
        public virtual ActionResult Index()
        {
            return RedirectToAction(ShopActionNames.Index, ControllerNames.Shop);
        }
    }
}