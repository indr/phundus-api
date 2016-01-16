namespace Phundus.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Common;
    using IdentityAccess.Queries;
    using phiNdus.fundus.Web.ViewModels.Layout;

    public class LayoutController : ControllerBase
    {
        public IMembershipQueries MembershipQueries { get; set; }

        [Transaction]
        [AllowAnonymous]
        [ChildActionOnly]
        public virtual ActionResult NavBar()
        {
            var model = new NavBarModel();

            var user = System.Web.Security.Membership.GetUser();
            if (user == null)
                return PartialView("_NavBar", model);

            if (user.ProviderUserKey != null)
            {
                var providerUserKey = new ProviderUserKey(user.ProviderUserKey);
                model.UserId = providerUserKey.UserGuid.Id.ToString("D");

                foreach (var each in MembershipQueries.ByUserId(providerUserKey.UserGuid.Id))
                {
                    model.Memberships.Add(each);
                }
            }

            return PartialView("_NavBar", model);
        }
    }
}