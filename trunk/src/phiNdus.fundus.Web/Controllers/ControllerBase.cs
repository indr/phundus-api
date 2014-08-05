namespace Phundus.Web.Controllers
{
    using System;
    using System.IO;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Security;
    using Core.Cqrs;
    using NHibernate;

    public abstract class ControllerBase : Controller
    {
        public IIdentity Identity { get; set; }

        public Func<ISession> SessionFact { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        protected string SessionId
        {
            get { return Session.SessionID; }
        }

        protected int? CurrentOrganizationId
        {
            get
            {
                if (Session["OrganizationId"] == null)
                    return null;
                return Convert.ToInt32(Session["OrganizationId"]);
            }

            set { Session["OrganizationId"] = value; }
        }

        protected int CurrentUserId
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                    throw new AuthenticationException();

                var userId = user.ProviderUserKey;
                return Convert.ToInt32(userId);
            }
        }

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