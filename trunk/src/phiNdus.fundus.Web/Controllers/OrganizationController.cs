using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    public class OrganizationController : Controller
    {
        public IOrganizationRepository Organizations { get; set; }
        public IUserRepository Users { get; set; }

        public ActionResult Index()
        {
            return View("tbd");
        }

        public ActionResult Id(int id)
        {
            return View("tbd");
        }

        public ActionResult Select(int id)
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

        public ActionResult Search()
        {
            return View("tbd");
        }

        public ActionResult Establish()
        {
            return View("tbd");
        }

    }
}
