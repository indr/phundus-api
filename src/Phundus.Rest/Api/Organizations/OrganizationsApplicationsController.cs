namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using IdentityAccess.Organizations.Commands;
    using IdentityAccess.Queries;
    using IdentityAccess.Queries.ReadModels;
    using Newtonsoft.Json;

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
        public virtual OrganizationsApplicationsPostOkResponseContent Post(Guid organizationId)
        {
            var applicationId = Guid.NewGuid();
            Dispatch(new ApplyForMembership(CurrentUserGuid, applicationId, CurrentUserGuid, organizationId));

            return new OrganizationsApplicationsPostOkResponseContent
            {
                ApplicationId = applicationId
            };
        }

        [DELETE("{applicationId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, Guid applicationId)
        {
            Dispatch(new RejectMembershipApplication(CurrentUserGuid, applicationId));

            return NoContent();
        }
    }

    public class OrganizationsApplicationsGetOkResponseContent : List<MembershipApplicationDto>
    {
        public OrganizationsApplicationsGetOkResponseContent(IEnumerable<MembershipApplicationDto> collection)
            : base(collection)
        {
        }
    }

    public class OrganizationsApplicationsPostOkResponseContent
    {
        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
    }
}