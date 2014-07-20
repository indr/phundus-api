namespace phiNdus.fundus.Web.Controllers.WebApi.Organizations
{
    using System.Web.Http;
    using Phundus.Core.Cqrs;
    using Phundus.Core.OrganisationCtx.Commands;
    using Phundus.Core.OrganisationCtx.ReadModel;

    public class MembershipApplicationsController : ApiController
    {
        public ICommandDispatcher Dispatcher { get; set; }

        public MembershipApplicationsReadModel MembershipApplications { get; set; }

        public MembershipApplicationDtos Get(int orgId)
        {
            return MembershipApplications.ByOrganization(orgId);
        }

        public void Post(int orgId, MembershipApplicationDto dto)
        {
            Dispatcher.Dispatch(new ApplyForMembership {MemberId = dto.UserId, OrganizationId = orgId});
        }
    }
}