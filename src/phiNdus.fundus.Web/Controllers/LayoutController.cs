namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Phundus.Core.IdentityAndAccess.Queries;
    using ViewModels.Layout;

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

            foreach (var each in MembershipQueries.ByUserId(Convert.ToInt32(user.ProviderUserKey)))
            {
                if ((model.Selected == null) || (each.OrganizationId == OrganizationId))
                {
                    model.Selected = each;
                    OrganizationId = each.OrganizationId;
                }


                model.Memberships.Add(each);
            }

            return PartialView("_NavBar", model);
        }
    }
}