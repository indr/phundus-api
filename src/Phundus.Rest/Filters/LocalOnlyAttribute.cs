namespace Phundus.Rest.Filters
{
    using System.Web;
    using System.Web.Http.Controllers;
    using Common;

    public class LocalOnlyAttribute : System.Web.Http.Filters.AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var isLocal = HttpContext.Current != null && HttpContext.Current.Request.IsLocal;
            if (!isLocal)
            {
                throw new AuthorizationException();
            }
        }
    }
}