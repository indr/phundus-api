namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
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
                model.UserId = user.ProviderUserKey.ToString();

            foreach (var each in MembershipQueries.ByUserId(Convert.ToInt32(user.ProviderUserKey)))
            {                
                model.Memberships.Add(each);
            }

            return PartialView("_NavBar", model);
        }
    }
}