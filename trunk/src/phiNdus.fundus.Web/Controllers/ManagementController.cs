namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;

    public class ManagementController : ControllerBase
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}