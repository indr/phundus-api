﻿namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Security.Authentication;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Domain.Repositories;
    using Models.Organization;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class OrganizationController : ControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }
        public IUserRepository Users { get; set; }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Id(int id)
        {
            return Home(id, null);
        }

        [Transaction]
        public virtual ActionResult Home(int id, string name)
        {
            var organization = Organizations.FindById(id);
            if (organization == null)
                throw new Exception(
                    String.Format(
                        "Die Organisation mit der Id {0} und/oder dem Namen \"{1}\" konnte nicht gefunden werden.",
                        id, name));

            var user = Users.FindByEmail(User.Identity.Name);
            var model = new OrganizationModel(organization);
            if (user != null)
            {
                bool isMemberOf = user.IsMemberOf(organization);
                model.HasOptionJoin = !isMemberOf;
                model.HasOptionLeave = isMemberOf;
            }
            

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
    }
}