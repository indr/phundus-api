namespace Phundus.Rest.Controllers.Organizations
{
    #region

    using System;
    using System.Web.Http;
    using Core.Cqrs;
    using Core.OrganisationCtx.Commands;
    using Core.OrganisationCtx.ReadModel;

    #endregion

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

        public void Delete(int organization, Guid id)
        {
            Dispatcher.Dispatch(new RejectMembershipRequest {RequestId = id});
        }

        public void Patch(int organization, Guid id)
        {
            Dispatcher.Dispatch(new ApproveMembershipRequest {RequestId = id});
        }
    }
}