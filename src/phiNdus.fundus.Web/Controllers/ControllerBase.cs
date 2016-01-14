namespace Phundus.Web.Controllers
{
    using System;
    using System.IO;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common;
    using Cqrs;
    using NHibernate;

    public abstract class ControllerBase : Controller
    {
        public IIdentity Identity { get; set; }

        public Func<ISession> SessionFact { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        protected int CurrentUserId
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                    throw new AuthenticationException();

                var userKey = new ProviderUserKey(user.ProviderUserKey);
                return userKey.UserId.Id;
            }
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
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