namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using NHibernate;
    using phiNdus.fundus.Web.ViewModels.Layout;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;

    public class LayoutController : ControllerBase
    {
        public IUserRepository Users { get; set; }

        [ChildActionOnly]
        [Transaction]
        public virtual ActionResult NavBar()
        {
            var model = new NavBarModel();
            var user = Users.FindByEmail(Identity.Name);
            if (user != null)
            {
                model.Selected = user.SelectedOrganization;
                //foreach (var each in user.Memberships)
                //{
                //    model.Organizations.Add(each.Organization);
                //    NHibernateUtil.Initialize(each.Organization);
                //}
            }
            return PartialView("_NavBar", model);
        }
    }
}