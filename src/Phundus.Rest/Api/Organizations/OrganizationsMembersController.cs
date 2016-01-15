namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Organizations.Commands;
    using IdentityAccess.Queries.ReadModels;
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
        public virtual QueryOkResponseContent<Member> Get(Guid organizationId)
        {
            var username = (string) null;
            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("username"))
                username = queryParams["username"];

            var result = _memberQueries.Query(CurrentUserGuid, organizationId, username);
            return new QueryOkResponseContent<Member>(result.Select(s => new Member
            {
                ApprovalDate = s.ApprovalDate,
                EmailAddress = s.EmailAddress,
                FirstName = s.FirstName,
                FullName = s.FullName,
                Guid = s.Guid,
                Id=s.Id,
                IsLocked = s.IsLocked,
                IsManager = s.Role == 2,
                JsNumber = s.JsNumber,
                LastName = s.LastName,
                Role = s.Role
            }).ToList());
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(Guid organizationId,
            OrganizationsMembersPostRequestContent requestContent)
        {
            Dispatcher.Dispatch(new ApproveMembershipApplication(CurrentUserGuid, requestContent.ApplicationId));

            return NoContent();
        }

        [PUT("{memberId}")]
        [Transaction]
        public virtual HttpResponseMessage Put(Guid organizationId, Guid memberId,
            OrganizationsMembersPutRequestContent requestContent)
        {
            Dispatcher.Dispatch(new ChangeMembersRole
            {
                OrganizationId = organizationId,
                InitiatorId = CurrentUserGuid,
                MemberId = new UserGuid(memberId),
                Role = requestContent.Role
            });

            return NoContent();
        }

        [PATCH("{memberId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid organizationId, Guid memberId,
            OrganizationsMembersPatchRequestContent requestContent)
        {
            if (requestContent.IsManager.HasValue)
            {
                Dispatch(new ChangeMembersRole
                {
                    OrganizationId = organizationId,
                    InitiatorId = CurrentUserGuid,
                    MemberId = new UserGuid(memberId),
                    Role = requestContent.IsManager.Value ? 2 : 1
                });
            }
            if (requestContent.IsLocked.HasValue)
            {
                if (requestContent.IsLocked.Value)
                {
                    Dispatcher.Dispatch(new LockMember
                    {
                        InitiatorId = CurrentUserGuid,
                        MemberId = new UserGuid(memberId),
                        OrganizationId = organizationId
                    });
                }
                else
                {
                    Dispatcher.Dispatch(new UnlockMember
                    {
                        InitiatorId = CurrentUserGuid,
                        MemberId = new UserGuid(memberId),
                        OrganizationId = organizationId
                    });
                }
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
    }

    public class OrganizationsMembersPutRequestContent
    {
        [JsonProperty("role")]
        public int Role { get; set; }
    }
}