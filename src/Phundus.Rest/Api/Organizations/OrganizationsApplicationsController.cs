namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Common.Resources;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Newtonsoft.Json;    

    [RoutePrefix("api/organizations/{organizationId}/applications")]
    public class OrganizationsApplicationsController : ApiControllerBase
    {
        private readonly IMembershipApplicationQueryService _membershipApplicationQueryService;

        public OrganizationsApplicationsController(IMembershipApplicationQueryService membershipApplicationQueryService)
        {            
            _membershipApplicationQueryService = membershipApplicationQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<MembershipApplicationData> Get(Guid organizationId)
        {
            var results = _membershipApplicationQueryService.FindPending(CurrentUserId, new OrganizationId(organizationId));
            return new QueryOkResponseContent<MembershipApplicationData>(results);
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

    public class OrganizationsApplicationsPostOkResponseContent
    {
        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
    }
}