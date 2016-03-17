namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Application;
    using Castle.Transactions;
    using Projections;

    [AllowAnonymous]
    public class Html5ModeController : Controller
    {
        private readonly IUrlMapQueryService _urlMapQueryService;

        public Html5ModeController(IUrlMapQueryService urlMapQueryService)
        {
            if (urlMapQueryService == null) throw new ArgumentNullException("urlMapQueryService");
            _urlMapQueryService = urlMapQueryService;
        }

        [Transaction]
        public virtual ActionResult Index(string url)
        {
            var friendlyUrl = _urlMapQueryService.FindByUrl(url);

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