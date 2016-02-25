namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Projections;

    [AllowAnonymous]
    public class Html5ModeController : Controller
    {
        private readonly IUrlMapQueries _urlMapQueries;

        public Html5ModeController(IUrlMapQueries urlMapQueries)
        {
            if (urlMapQueries == null) throw new ArgumentNullException("urlMapQueries");
            _urlMapQueries = urlMapQueries;
        }

        [Transaction]
        public virtual ActionResult Index(string url)
        {
            var friendlyUrl = _urlMapQueries.FindByUrl(url);

            if (friendlyUrl == null)
                return Redirect(@"/#/404");

            if (friendlyUrl.OrganizationId.HasValue)
                return Redirect(@"/#/organizations/" + friendlyUrl.OrganizationId.Value);

            if (friendlyUrl.UserId.HasValue)
                return Redirect(@"/#/users/" + friendlyUrl.UserId.Value);

            return Redirect(@"/#/404");
        }
    }
}