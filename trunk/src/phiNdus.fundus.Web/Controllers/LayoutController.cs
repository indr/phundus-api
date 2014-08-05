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

            foreach (var each in MembershipQueries.ByUserId(Convert.ToInt32(user.ProviderUserKey)))
            {
                if ((model.Selected == null) || (each.OrganizationId == CurrentOrganizationId))
                    model.Selected = each;
                model.Memberships.Add(each);
            }

            if (model.Selected != null)
                CurrentOrganizationId = model.Selected.OrganizationId;

            return PartialView("_NavBar", model);
        }
    }
}