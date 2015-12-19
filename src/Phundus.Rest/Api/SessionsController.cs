namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Authentication;
    using System.Web.Http;
    using System.Web.Security;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("/api/sessions")]
    public class SessionsController : ApiControllerBase
    {
        private IUserQueries _userQueries;
        private IMembershipQueries _membershipQueries;

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
        public virtual object Post(SessionsPostRequestContent requestContent)
        {
            if (!Membership.ValidateUser(requestContent.Username, requestContent.Password))
                throw new AuthenticationException();

            FormsAuthentication.SetAuthCookie(requestContent.Username, false);

            var user = _userQueries.ByUserName(requestContent.Username);


            var memberships = _membershipQueries.ByUserId(user.Id)
                .Select(each => new Memberships
                {
                    IsManager = each.MembershipRole == "manager",
                    IsSelected = false,
                    OrganizationId = each.OrganizationId.ToString(CultureInfo.InvariantCulture),
                    OrganizationName = each.OrganizationName,
                    OrganizationUrl = each.OrganizationUrl
                }).ToList();

            
            return new SessionsPostOkResponseContent
            {
                Memberships = memberships,
                Role = new Role
                {
                    BitMask = 5,
                    Title = "todo"
                },
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                Username = Identity.Name
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
        public IList<Memberships> Memberships { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class Memberships
    {
        [JsonProperty("organizationId")]
        public string OrganizationId { get; set; }

        [JsonProperty("organizationName")]
        public string OrganizationName { get; set; }

        [JsonProperty("organizationUrl")]
        public string OrganizationUrl { get; set; }

        [JsonProperty("isManager")]
        public bool? IsManager { get; set; }

        [JsonProperty("isSelected")]
        public bool? IsSelected { get; set; }
    }

    public class Role
    {
        [JsonProperty("bitMask")]
        public int BitMask { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}