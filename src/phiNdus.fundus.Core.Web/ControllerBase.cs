using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web
{
    public abstract class ControllerBase : Controller
    {
        // Source: http://stackoverflow.com/questions/2374046/returning-an-editortemplate-as-a-partialview-in-an-action-result-asp-net-mvc-2
        protected PartialViewResult EditorFor(object model)
        {
            return PartialView("~/Views/Shared/EditorTemplates/" + model.GetType().Name + ".cshtml", model);
        }

        // Source: http://stackoverflow.com/questions/2374046/returning-an-editortemplate-as-a-partialview-in-an-action-result-asp-net-mvc-2
        protected PartialViewResult DisplayFor(object model)
        {
            return PartialView("~/Views/Shared/DisplayTemplates/" + model.GetType().Name + ".cshtml", model);
        }
    }
}
