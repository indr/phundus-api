namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Phundus.Core.IdentityAndAccessCtx.Queries;
    using Phundus.Core.OrganizationAndMembershipCtx.Queries;
    using ViewModels.Layout;

    public class LayoutController : ControllerBase
    {
        public IUserQueries UserQueries { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        [ChildActionOnly]
        [Transaction]
        public virtual ActionResult NavBar()
        {
            var model = new NavBarModel();
            var user = UserQueries.ByEmail(Identity.Name);
            if (user == null)
                return PartialView("_NavBar", model);

            var selectedOrganizationId = 0;
            if (Session["OrganizationId"] != null)
                selectedOrganizationId = Convert.ToInt32(Session["OrganizationId"]);

            foreach (var each in MembershipQueries.ByMemberId(user.Id))
            {
                if ((model.Selected == null) || (each.OrganizationId == selectedOrganizationId))
                    model.Selected = each;
                
                model.Memberships.Add(each);
            }

            return PartialView("_NavBar", model);
        }
    }
}