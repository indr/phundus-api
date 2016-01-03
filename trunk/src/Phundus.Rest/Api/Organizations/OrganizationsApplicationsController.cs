namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
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
        public virtual OrganizationsApplicationsGetOkResponseContent Get(Guid organizationId)
        {
            var result = MembershipApplicationQueries.PendingByOrganizationId(organizationId);
            return new OrganizationsApplicationsGetOkResponseContent(result);
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(Guid organizationId)
        {
            Dispatch(new ApplyForMembership {ApplicantId = CurrentUserId.Id, OrganizationId = organizationId});

            return CreateNoContentResponse();
        }

        [DELETE("{applicationId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, Guid applicationId)
        {
            Dispatch(new RejectMembershipApplication {ApplicationId = applicationId, InitiatorId = CurrentUserId.Id});

            return CreateNoContentResponse();
        }
    }

    public class OrganizationsApplicationsGetOkResponseContent : List<MembershipApplicationDto>
    {
        public OrganizationsApplicationsGetOkResponseContent(IEnumerable<MembershipApplicationDto> collection)
            : base(collection)
        {
        }
    }
}