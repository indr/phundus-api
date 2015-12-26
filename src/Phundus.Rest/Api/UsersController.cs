namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMembershipQueries _membershipQueries;
        private readonly IUserQueries _userQueries;

        public UsersController(IUserQueries userQueries, IMembershipQueries membershipQueries)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(membershipQueries, "MembershipQueries must be provided.");

            _userQueries = userQueries;
            _membershipQueries = membershipQueries;
        }

        [GET("{userId}")]
        [Transaction]
        public virtual UsersGetOkResponseContent Get(int userId)
        {
            var user = _userQueries.ById(userId);
            if ((user == null) || (user.Id != CurrentUserId))
                throw new HttpException((int) HttpStatusCode.NotFound, "User not found.");

            var memberships = _membershipQueries.ByUserId(user.Id);

            return new UsersGetOkResponseContent(user, memberships);
        }
    }

    public class UsersGetOkResponseContent
    {
        public UsersGetOkResponseContent(UserDto user, IEnumerable<MembershipDto> memberships)
        {
            UserId = user.Id.ToString(CultureInfo.InvariantCulture);
            Username = user.Email;
            FullName = user.FirstName + " " + user.LastName;
            EmailAddress = user.Email;
            Memberships = new List<Memberships>();

            foreach (var each in memberships)
            {
                Memberships.Add(new Memberships
                {
                    OrganizationId = each.OrganizationId.ToString(CultureInfo.InvariantCulture),
                    OrganizationName = each.OrganizationName,
                    OrganizationUrl = each.OrganizationUrl
                });
            }
        }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        [JsonProperty("memberships")]
        public IList<Memberships> Memberships { get; set; }

        [JsonProperty("store")]
        public Store Store { get; set; }
    }

    public class Store
    {
        [JsonProperty("storeId")]
        public string StoreId { get; set; }
    }
}