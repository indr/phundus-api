namespace phiNdus.fundus.Web.Controllers
{
    using System.Security.Authentication;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Transactions;
    using phiNdus.fundus.Domain.Repositories;
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

        [Transaction]
        public virtual ActionResult Id(int id)
        {
            return View("tbd");
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
                uow.TransactionalFlush();
            }
            return Id(id);
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
            //return View();
            return View("tbd");
        }

        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Settings()
        {
            return View("tbd");
        }
    }
}