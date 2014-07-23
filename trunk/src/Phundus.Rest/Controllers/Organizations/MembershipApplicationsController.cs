﻿namespace Phundus.Rest.Controllers.Organizations
{
    using System;
    using Core.OrganizationAndMembershipCtx.Commands;
    using Core.OrganizationAndMembershipCtx.Queries;

    public class MembershipApplicationsController : ApiControllerBase
    {
        public IMembershipApplicationQueries MembershipApplications { get; set; }

        public MembershipApplicationDtos Get(int organization)
        {
            return MembershipApplications.PendingByOrganizationId(organization);
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