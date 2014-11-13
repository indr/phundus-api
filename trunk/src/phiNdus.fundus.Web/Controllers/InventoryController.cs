namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;

    public class InventoryController : ControllerBase
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}