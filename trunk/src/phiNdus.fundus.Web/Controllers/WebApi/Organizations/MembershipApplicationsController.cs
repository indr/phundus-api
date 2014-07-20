namespace phiNdus.fundus.Web.Controllers.WebApi.Organizations
{
    using System.Web.Http;
    using Castle.Transactions;
    using Phundus.Core.Cqrs;
    using Phundus.Core.OrganisationCtx.Commands;
    using Phundus.Core.OrganisationCtx.ReadModel;

    public class MembershipApplicationsController : ApiController
    {
        public ICommandDispatcher Dispatcher { get; set; }

        public IMembershipApplicationsReadModel MembershipApplications { get; set; }

        public MembershipApplicationDtos Get(int organization)
        {
            return MembershipApplications.ByOrganization(organization);
        }

        public void Post(int organization, MembershipApplicationDto dto)
        {
            Dispatcher.Dispatch(new ApplyForMembership {MemberId = dto.UserId, OrganizationId = organization});
        }
    }
}