using System.Linq;
using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Helpers
{
    public static class ActiveTabExtension
    {
        /// <summary>
        /// http://blog.tomasjansson.com/2010/09/asp-net-mvc-helper-for-active-tab-in-tab-menu/
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="activeController"></param>
        /// <param name="activeActions"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string ActiveTab(this HtmlHelper helper, string activeController, string[] activeActions,
                                       string cssClass = "active")
        {
            string currentAction = helper.ViewContext.Controller.
                ValueProvider.GetValue("action").RawValue.ToString();
            string currentController = helper.ViewContext.Controller.
                ValueProvider.GetValue("controller").RawValue.ToString();

            string cssClassToUse = currentController == activeController
                                   && activeActions.Contains(currentAction)
                                       ? cssClass
                                       : string.Empty;
            return cssClassToUse;
        }
    }
}