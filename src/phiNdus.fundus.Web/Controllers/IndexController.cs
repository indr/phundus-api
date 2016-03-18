namespace Phundus.Web.Controllers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;

    [AllowAnonymous]
    public class IndexController : Controller
    {
        public virtual ActionResult Index()
        {            
            var content = System.IO.File.ReadAllText(Server.MapPath("~/index.html"));

            var idx = content.IndexOf(@"<link", StringComparison.InvariantCulture);
            if (idx < 0)
                throw new Exception(
                    "Fehler beim Einfügen des base-Tags: Die Einfügeposition, resp. ein link-Tag konnte nicht gefunden werden. Siehe Methode HomeController.Index().");

            content = content.Insert(idx, RenderPartialViewToString("_BaseUrl") + " ");
            return Content(content, "text/html", Encoding.UTF8);
        }

        private string RenderPartialViewToString(string viewName, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}