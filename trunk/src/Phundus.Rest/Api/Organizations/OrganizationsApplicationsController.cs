namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/applications")]
    public class OrganizationsApplicationsController : ApiControllerBase
    {
        private readonly IMembershipApplicationQueries _membershipApplicationQueries;

        public OrganizationsApplicationsController(IMembershipApplicationQueries membershipApplicationQueries)
        {
            if (membershipApplicationQueries == null) throw new ArgumentNullException("membershipApplicationQueries");
            _membershipApplicationQueries = membershipApplicationQueries;
        }

        [GET("")]
        [Transaction]
        public virtual OrganizationsApplicationsGetOkResponseContent Get(Guid organizationId)
        {
            var results = _membershipApplicationQueries.FindPending(CurrentUserId, new OrganizationId(organizationId));
            return new OrganizationsApplicationsGetOkResponseContent(results);
        }

        [POST("")]        
        public virtual OrganizationsApplicationsPostOkResponseContent Post(Guid organizationId)
        {
            var applicationId = new MembershipApplicationId();
            Dispatch(new ApplyForMembership(CurrentUserId, applicationId, CurrentUserId, new OrganizationId(organizationId)));

            return new OrganizationsApplicationsPostOkResponseContent
            {
                ApplicationId = applicationId.Id
            };
        }

        [DELETE("{applicationId}")]        
        public virtual HttpResponseMessage Delete(Guid organizationId, Guid applicationId)
        {
            Dispatch(new RejectMembershipApplication(CurrentUserId, new MembershipApplicationId(applicationId)));

            return NoContent();
        }
    }

    public class OrganizationsApplicationsGetOkResponseContent : List<IMembershipApplication>
    {
        public OrganizationsApplicationsGetOkResponseContent(IEnumerable<IMembershipApplication> collection)
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