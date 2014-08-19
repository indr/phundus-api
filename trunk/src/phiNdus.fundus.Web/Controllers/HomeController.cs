namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return RedirectToAction(ShopActionNames.Index, ControllerNames.Shop);
        }

        [AllowAnonymous]
        public virtual ActionResult Throw()
        {
            throw new Exception("Exception zur Diagnose!");
        }

        [AllowAnonymous]
        public virtual ActionResult NotFound()
        {
            return HttpNotFound("HttpNotFound zur Diagnose!");
        }
    }
}