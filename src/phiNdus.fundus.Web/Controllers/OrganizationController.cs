namespace Phundus.Web.Controllers
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

        public IRelationshipQueries RelationshipQueries { get; set; }

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
            var organization = OrganizationQueries.ById(id);

            if (organization == null)
                throw new Exception(
                    String.Format(
                        "Die Organisation mit der Id {0} und/oder dem Namen \"{1}\" konnte nicht gefunden werden.",
                        id, name));

            var model = new OrganizationModel(organization);

            var user = System.Web.Security.Membership.GetUser();

            if (user != null)
            {
                model.Relationship = RelationshipQueries.ByMemberIdForOrganizationId(
                    Convert.ToInt32(user.ProviderUserKey), organization.Id);
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

            CurrentOrganizationId = organization.Id;
            return RedirectToRoute("Organization", new {name = organization.Url});
        }
    }
}