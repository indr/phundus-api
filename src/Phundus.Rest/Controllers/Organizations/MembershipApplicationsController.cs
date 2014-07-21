namespace Phundus.Rest.Controllers.Organizations
{
    #region

    using System;
    using Core.OrganisationCtx.Commands;
    using Core.OrganisationCtx.ReadModel;

    #endregion

    public class MembershipApplicationsController : ApiControllerBase
    {
        public IMembershipApplicationsReadModel MembershipApplications { get; set; }

        public MembershipApplicationDtos Get(int organization)
        {
            return MembershipApplications.ByOrganization(organization);
        }

        public void Post(int organization, MembershipApplicationDto dto)
        {
            Dispatch(new ApplyForMembership {MemberId = dto.UserId, OrganizationId = organization});
        }

        public void Delete(int organization, Guid id)
        {
            Dispatch(new RejectMembershipRequest {RequestId = id});
        }

        public void Patch(int organization, Guid id)
        {
            Dispatch(new ApproveMembershipRequest {RequestId = id});
        }
    }
}