namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;

    public class DocumentsController : ControllerBase
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}