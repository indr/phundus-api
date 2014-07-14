namespace phiNdus.fundus.Web.Controllers.WebApi.Organizations
{
    using System;
    using System.Web.Http;
    using Phundus.Core.Cqrs;
    using Phundus.Core.OrganisationCtx.Commands;

    public class MembershipApplicationsController : ApiController
    {
        public ICommandDispatcher Dispatcher { get; set; }

        public void Post(int orgId, MembershipApplicationDoc dto)
        {
            Dispatcher.Dispatch(new ApplyForMembership {MemberId = dto.UserId, OrganizationId = orgId});
        }
    }

    public class MembershipApplicationDoc
    {
        private DateTime _createdOn = DateTime.UtcNow;

        public int OrgId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }
    }
}