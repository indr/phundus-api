namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Security.Authentication;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Domain.Repositories;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class OrganizationController : ControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }
        public IUserRepository Users { get; set; }

        [Transaction]
        public virtual ActionResult Index()
        {
            return View("tbd");
        }

        public virtual ActionResult Id(int id)
        {
            return Home(id, null);
        }

        [Transaction]
        public virtual ActionResult Home(int id, string name)
        {
            var model = Organizations.FindById(id);
            if (model == null)
                throw new Exception(
                    String.Format(
                        "Die Organisation mit der Id {0} und/oder dem Namen \"{1}\" konnte nicht gefunden werden.",
                        id, name));
            return View(model);
        }

        [Authorize]
        [Transaction]
        public virtual ActionResult Select(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var organization = Organizations.FindById(id);
                if (organization == null)
                    throw new HttpException(404, "Organisation nicht gefunden.");

                var user = Users.FindByEmail(User.Identity.Name);
                if (user == null)
                    throw new AuthenticationException("Um eine Organisation auszuwählen, müssen Sie sich anmelden.");

                user.SelectOrganization(organization);

                Session["OrganizationId"] = organization.Id;
                uow.TransactionalFlush();

                return RedirectToRoute("Organization", new {name = organization.Url});
            }
        }

        [Transaction]
        public virtual ActionResult Search()
        {
            return View("tbd");
        }

        [Transaction]
        public virtual ActionResult Establish()
        {
            return View("tbd");
        }

        [Authorize(Roles = "Chief")]
        public virtual ActionResult Members()
        {
            return View();
            //return View("tbd");
        }

        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Settings()
        {
            return View("tbd");
        }
    }
}