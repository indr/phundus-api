namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/members")]
    public class OrganizationsMembersController : ApiControllerBase
    {
        private readonly IMemberQueries _memberQueries;

        public OrganizationsMembersController(IMemberQueries memberQueries)
        {
            AssertionConcern.AssertArgumentNotNull(memberQueries, "MemberQueries must be provided.");

            _memberQueries = memberQueries;
        }

        [GET("")]
        [Transaction]
        public virtual OrganizationsMembersQueryOkResponseContent Get(Guid organizationId)
        {
            var username = (string) null;
            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("username"))
                username = queryParams["username"];

            var result = _memberQueries.Query(CurrentUserId, organizationId, username);
            return new OrganizationsMembersQueryOkResponseContent(result);
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(Guid organizationId,
            OrganizationsMembersPostRequestContent requestContent)
        {
            Dispatcher.Dispatch(new ApproveMembershipApplication
            {
                InitiatorId = CurrentUserId.Id,
                ApplicationId = requestContent.ApplicationId
            });

            return CreateNoContentResponse();
        }

        [PUT("{memberId}")]
        [Transaction]
        public virtual HttpResponseMessage Put(Guid organizationId, int memberId,
            OrganizationsMembersPutRequestContent requestContent)
        {
            Dispatcher.Dispatch(new ChangeMembersRole
            {
                OrganizationId = organizationId,
                InitiatorId = CurrentUserId.Id,
                MemberId = memberId,
                Role = requestContent.Role
            });

            return CreateNoContentResponse();
        }
    }

    public class OrganizationsMembersPostRequestContent
    {
        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
    }

    public class OrganizationsMembersPutRequestContent
    {
        [JsonProperty("role")]
        public int Role { get; set; }
    }

    public class OrganizationsMembersQueryOkResponseContent : List<MemberDto>
    {
        public OrganizationsMembersQueryOkResponseContent(IEnumerable<MemberDto> collection) : base(collection)
        {
        }
    }
}