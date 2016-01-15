namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
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
        public virtual ActionResult Index(Guid id)
        {
            var isManager = _memberInRole.IsActiveChief(id, new UserGuid(CurrentUserGuid));
            return View(new ManagementModel {OrganizationId = id, IsManager = isManager});
        }
    }
}