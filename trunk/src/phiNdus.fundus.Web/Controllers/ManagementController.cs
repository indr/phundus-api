namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using phiNdus.fundus.Web.Models;

    public class ManagementController : ControllerBase
    {
        private readonly IMemberInRole _memberInRole;

        public ManagementController(IMemberInRole memberInRole)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "Member in role must be provided.");

            _memberInRole = memberInRole;
        }

        [Transaction]
        public virtual ActionResult Index(int id)
        {
            var isManager = _memberInRole.IsActiveChief(id, CurrentUserId);
            return View(new ManagementModel {OrganizationId = id, IsManager = isManager});
        }
    }
}