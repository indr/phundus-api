namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return RedirectToAction(ShopActionNames.Index, ControllerNames.Shop);
        }
    }
}