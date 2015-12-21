namespace Phundus.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using phiNdus.fundus.Web.Models.Organization;

    public class OrganizationController : ControllerBase
    {
        private IMemberInRole _memberInRole;

        public OrganizationController(IMemberInRole memberInRole)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "Member in role must be provided.");

            _memberInRole = memberInRole;
        }

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

            var isManager = Identity.IsAuthenticated && _memberInRole.IsActiveChief(id, CurrentUserId);
            var model = new OrganizationModel(organization, isManager);

            return View("Home", model);
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