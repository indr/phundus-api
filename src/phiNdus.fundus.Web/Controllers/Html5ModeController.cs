namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Projections;

    [AllowAnonymous]
    public class Html5ModeController : Controller
    {
        private readonly IFriendlyUrlQueries _friendlyUrlQueries;

        public Html5ModeController(IFriendlyUrlQueries friendlyUrlQueries)
        {
            if (friendlyUrlQueries == null) throw new ArgumentNullException("friendlyUrlQueries");
            _friendlyUrlQueries = friendlyUrlQueries;
        }

        [Transaction]
        public virtual ActionResult Index(string url)
        {
            var friendlyUrl = _friendlyUrlQueries.FindByUrl(url);

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