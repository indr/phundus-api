namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/members")]
    public class OrganizationsMembersController : ApiControllerBase
    {
        private readonly IMemberQueries _memberQueries;

        public OrganizationsMembersController(IMemberQueries memberQueries)
        {
            _memberQueries = memberQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<Member> Get(Guid organizationId)
        {
            var username = (string) null;
            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("username"))
                username = queryParams["username"];

            var result = _memberQueries.Query(CurrentUserId, organizationId, username);
            return new QueryOkResponseContent<Member>(result.Select(s => new Member
            {
                ApprovalDate = s.ApprovalDate,
                EmailAddress = s.EmailAddress,
                FirstName = s.FirstName,
                FullName = s.FullName,
                Guid = s.Guid,
                IsLocked = s.IsLocked,
                IsManager = s.Role == 2,
                JsNumber = s.JsNumber,
                LastName = s.LastName,
                RecievesEmailNotifications = s.RecievesEmailNotifications,
                Role = s.Role
            }).ToList());
        }

        [POST("")]
        public virtual HttpResponseMessage Post(Guid organizationId,
            OrganizationsMembersPostRequestContent requestContent)
        {
            Dispatch(new ApproveMembershipApplication(CurrentUserId,
                new MembershipApplicationId(requestContent.ApplicationId)));

            return NoContent();
        }

        [PUT("{memberId}")]
        public virtual HttpResponseMessage Put(Guid organizationId, Guid memberId,
            OrganizationsMembersPutRequestContent requestContent)
        {
            Dispatch(new ChangeMembersRole(CurrentUserId, new OrganizationId(organizationId),
                new UserId(memberId), (MemberRole) requestContent.Role));

            return NoContent();
        }

        [PATCH("{memberId}")]
        public virtual HttpResponseMessage Patch(Guid organizationId, Guid memberId,
            OrganizationsMembersPatchRequestContent requestContent)
        {
            if (requestContent.IsManager.HasValue)
            {
                Dispatch(new ChangeMembersRole(CurrentUserId, new OrganizationId(organizationId), new UserId(memberId),
                    requestContent.IsManager.Value ? MemberRole.Manager : MemberRole.Member));
            }
            if (requestContent.IsLocked.HasValue)
            {
                if (requestContent.IsLocked.Value)
                {
                    Dispatch(new LockMember(CurrentUserId, new OrganizationId(organizationId), new UserId(memberId)));
                }
                else
                {
                    Dispatch(new UnlockMember(CurrentUserId, new OrganizationId(organizationId), new UserId(memberId)));
                }
            }
            if (requestContent.RecievesEmailNotifications.HasValue)
            {
                Dispatch(new ChangeMemberRecievesEmailNotification(CurrentUserId, new OrganizationId(organizationId),
                    new UserId(memberId),
                    requestContent.RecievesEmailNotifications.Value));
            }

            return NoContent();
        }
    }

    public class OrganizationsMembersPostRequestContent
    {
        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
    }

    public class OrganizationsMembersPatchRequestContent
    {
        [JsonProperty("isManager")]
        public bool? IsManager { get; set; }

        [JsonProperty("isLocked")]
        public bool? IsLocked { get; set; }

        [JsonProperty("recievesEmailNotifications")]
        public bool? RecievesEmailNotifications { get; set; }
    }

    public class OrganizationsMembersPutRequestContent
    {
        [JsonProperty("role")]
        public int Role { get; set; }
    }
}