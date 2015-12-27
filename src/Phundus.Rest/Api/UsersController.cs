namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMembershipQueries _membershipQueries;
        private readonly IUserQueries _userQueries;
        private readonly IStoreQueries _storeQueries;

        public UsersController(IUserQueries userQueries, IMembershipQueries membershipQueries,
            IStoreQueries storeQueries)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(membershipQueries, "MembershipQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeQueries, "StoreQueries must be provided.");

            _userQueries = userQueries;
            _membershipQueries = membershipQueries;
            _storeQueries = storeQueries;
        }

        [GET("{userId}")]
        [Transaction]
        public virtual UsersGetOkResponseContent Get(int userId)
        {
            var user = _userQueries.ById(userId);
            if ((user == null) || (user.Id != CurrentUserId))
                throw new HttpException((int) HttpStatusCode.NotFound, "User not found.");

            var memberships = _membershipQueries.ByUserId(user.Id);
            var store = _storeQueries.FindByUserId(user.Guid);

            return new UsersGetOkResponseContent(user, memberships, store);
        }
    }

    public class UsersGetOkResponseContent
    {
        public UsersGetOkResponseContent(UserDto user, IEnumerable<MembershipDto> memberships, StoreDto store)
        {
            UserId = user.Id.ToString(CultureInfo.InvariantCulture);
            UserGuid = user.Guid;
            Username = user.Email;
            FullName = user.FirstName + " " + user.LastName;
            EmailAddress = user.Email;
            
            Memberships = new List<Memberships>();
            foreach (var each in memberships)
            {
                Memberships.Add(new Memberships
                {
                    OrganizationId = each.OrganizationGuid,
                    OrganizationName = each.OrganizationName,
                    OrganizationUrl = each.OrganizationUrl
                });
            }

            if (store != null)
            {
                Store = new Store
                {
                    StoreId = store.StoreId.ToString("N"),
                    Address = store.Address,
                    OpeningHours = store.OpeningHours
                };

                if (store.Latitude.HasValue && store.Longitude.HasValue)
                {
                    Store.Coordinate = new Coordinate()
                    {
                        Latitude = store.Latitude.Value,
                        Longitude = store.Longitude.Value
                    };
                }
            }
        }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userGuid")]
        public Guid UserGuid { get; set; }

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

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }

        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }

    public class Coordinate
    {
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }
    }
}