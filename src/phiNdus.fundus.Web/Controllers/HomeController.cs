namespace Phundus.Web.Controllers
{
    using System;
    using System.Text;
    using System.Web.Mvc;

    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            var content = System.IO.File.ReadAllText(Server.MapPath("~/index.html"));
            return Content(content, "text/html", Encoding.UTF8);
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