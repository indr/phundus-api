namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Models.Organization;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;
    using Phundus.Core.OrganizationAndMembershipCtx.Queries;
    using Phundus.Core.OrganizationAndMembershipCtx.Repositories;

    public class OrganizationController : ControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }
        public IUserRepository Users { get; set; }

        public IOrganizationQueries OrganizationQueries { get; set; }

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

            var user = Users.FindByEmail(Identity.Name);
            var model = new OrganizationModel(organization);
            if (user != null)
            {
                // TODO: Security and Access
                bool isMemberOf = false; //user.IsMemberOf(organization);
                model.HasOptionJoin = !isMemberOf;
                model.HasOptionLeave = isMemberOf;
            }


            return View(model);
        }

        [Authorize]
        [Transaction]
        public virtual ActionResult Select(int id)
        {
            var organization = OrganizationQueries.ById(id);

            if (organization == null)
                throw new HttpException(404, "Organisation nicht gefunden.");

            Session["OrganizationId"] = organization.Id;
            return RedirectToRoute("Organization", new {name = organization.Url});
        }
    }
}