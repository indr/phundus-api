﻿namespace Phundus.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using phiNdus.fundus.Web.Models.Organization;

    public class OrganizationController : ControllerBase
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult Id(int id)
        {
            return Home(id, null);
        }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Home(int id, string name)
        {
            var organization = OrganizationQueries.ById(id);

            if (organization == null)
                throw new Exception(
                    String.Format(
                        "Die Organisation mit der Id {0} und/oder dem Namen \"{1}\" konnte nicht gefunden werden.",
                        id, name));

            var model = new OrganizationModel(organization);

            return View(model);
        }

        [Transaction]
        public virtual ActionResult Select(int id)
        {
            var organization = OrganizationQueries.ById(id);

            if (organization == null)
                throw new HttpException(404, "Organisation nicht gefunden.");

            CurrentOrganizationId = organization.Id;
            return RedirectToRoute("Organization", new {name = organization.Url});
        }
    }
}