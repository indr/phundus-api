using System.IO;
using System.Web.Mvc;

namespace phiNdus.fundus.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected string SessionId { get { return Session.SessionID; } }

        // Source: http://stackoverflow.com/questions/2374046/returning-an-editortemplate-as-a-partialview-in-an-action-result-asp-net-mvc-2
        protected PartialViewResult EditorFor(object model)
        {
           
            return PartialView("~/Views/Shared/EditorTemplates/" + model.GetType().Name + ".cshtml", model);
        }

        protected PartialViewResult EditorFor(object model, string htmlFieldPrefix)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            return EditorFor(model);
        }

        
        protected PartialViewResult DisplayFor(object model)
        {
            return PartialView("~/Views/Shared/DisplayTemplates/" + model.GetType().Name + ".cshtml", model);
        }

        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
