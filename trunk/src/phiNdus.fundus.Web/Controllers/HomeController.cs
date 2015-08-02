namespace Phundus.Web.Controllers
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            var content = System.IO.File.ReadAllText(Server.MapPath("~/index.html"));

            var idx = content.IndexOf(@"<link", StringComparison.InvariantCulture);
            if (idx < 0)
                throw new Exception("Fehler beim Einfügen des base-Tags: Die Einfügeposition, resp. ein link-Tag konnte nicht gefunden werden. Siehe Methode HomeController.Index().");

            content = content.Insert(idx, RenderPartialViewToString("_BaseUrl") + " ");
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