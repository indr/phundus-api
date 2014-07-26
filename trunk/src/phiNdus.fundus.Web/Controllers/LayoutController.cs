namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Phundus.Core.IdentityAndAccess.Queries;
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

            foreach (var each in MembershipQueries.ByMemberId(user.Id))
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