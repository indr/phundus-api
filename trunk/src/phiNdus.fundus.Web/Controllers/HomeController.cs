namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;
    using phiNdus.fundus.Web;
    using ControllerBase = phiNdus.fundus.Web.Controllers.ControllerBase;

    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return RedirectToAction(ShopActionNames.Index, ControllerNames.Shop);
        }
    }
}