namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Authentication;
    using System.Web.Http;
    using System.Web.Security;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Auth;
    using Castle.Transactions;
    using Common;
    using ContentObjects;
    using IdentityAccess.Queries;
    using Integration.IdentityAccess;
    using Newtonsoft.Json;

    [RoutePrefix("/api/sessions")]
    public class SessionsController : ApiControllerBase
    {
        private readonly IMembershipQueries _membershipQueries;
        private readonly IUserQueries _userQueries;

        public SessionsController(IUserQueries userQueries, IMembershipQueries membershipQueries)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "User queries must be provided.");
            AssertionConcern.AssertArgumentNotNull(membershipQueries, "Membership queries must be provided.");

            _userQueries = userQueries;
            _membershipQueries = membershipQueries;
        }

        [POST("")]
        [Transaction]
        [AllowAnonymous]
        public virtual SessionsPostOkResponseContent Post(SessionsPostRequestContent requestContent)
        {
            if (!Membership.ValidateUser(requestContent.Username, requestContent.Password))
                throw new AuthenticationException();

            FormsAuthentication.SetAuthCookie(requestContent.Username, false);

            var user = _userQueries.FindByUsername(requestContent.Username);

            var memberships = _membershipQueries.ByUserId(user.UserGuid)
                .Select(each => new Memberships
                {
                    IsManager = each.MembershipRole == "Chief",
                    OrganizationId = each.OrganizationGuid,
                    OrganizationName = each.OrganizationName,
                    OrganizationUrl = each.OrganizationUrl
                }).ToList();


            var auth = new Auth();
            UserRole role = null;
            if (!auth.Roles.TryGetValue(user.RoleName.ToLowerInvariant(), out role))
                role = auth.Roles["user"];

            return new SessionsPostOkResponseContent
            {
                Memberships = memberships,
                Role = new Role
                {
                    BitMask = role.BitMask,
                    Title = role.Title
                },
                UserId = user.UserGuid,
                Username = user.EmailAddress
            };
        }

        [DELETE("")]
        [Transaction]
        [AllowAnonymous]
        public virtual void Delete()
        {
            FormsAuthentication.SignOut();
        }
    }

    public class SessionsPostRequestContent
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class SessionsPostOkResponseContent
    {
        [JsonProperty("memberships")]
        public List<Memberships> Memberships { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class Role
    {
        [JsonProperty("bitMask")]
        public int BitMask { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}