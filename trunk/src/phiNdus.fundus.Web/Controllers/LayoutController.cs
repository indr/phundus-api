namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using NHibernate;
    using phiNdus.fundus.Domain.Repositories;
    using phiNdus.fundus.Web.ViewModels.Layout;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class LayoutController : ControllerBase
    {
        public IUserRepository UserRepository { get; set; }

        [ChildActionOnly]
        [Transaction]
        public virtual ActionResult NavBar()
        {
            var resolved = GlobalContainer.Resolve<IUserRepository>();
            if (resolved == null)
                throw new Exception("IUserRepository nicht installiert.");
            var model = new NavBarModel();
            using (UnitOfWork.Start())
            {
                var user = UserRepository.FindByEmail(User.Identity.Name);
                if (user != null)
                {
                    model.Selected = user.SelectedOrganization;
                    foreach (var each in user.Memberships)
                    {
                        model.Organizations.Add(each.Organization);
                        NHibernateUtil.Initialize(each.Organization);
                    }
                }
            }

            return PartialView("_NavBar", model);
        }
    }
}