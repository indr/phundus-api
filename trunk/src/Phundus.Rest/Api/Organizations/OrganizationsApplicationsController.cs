namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;

    [RoutePrefix("api/organizations/{organizationId}/applications")]
    public class OrganizationsApplicationsController : ApiControllerBase
    {
        public IMembershipApplicationQueries MembershipApplicationQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual IList<MembershipApplicationDto> Get(int organizationId)
        {
            return MembershipApplicationQueries.PendingByOrganizationId(organizationId);
        }

        [POST("")]
        [Transaction]
        public virtual void Post(int organizationId)
        {
            Dispatch(new ApplyForMembership {ApplicantId = CurrentUserId, OrganizationId = organizationId});
        }

        [DELETE("{applicationId}")]
        [Transaction]
        public virtual void Delete(int organizationId, Guid applicationId)
        {
            Dispatch(new RejectMembershipApplication { ApplicationId = applicationId, InitiatorId = CurrentUserId });
        }

    }
}