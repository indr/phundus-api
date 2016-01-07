namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Commands;
    using Core.Inventory.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMembershipQueries _membershipQueries;
        private readonly IStoreQueries _storeQueries;
        private readonly IUserQueries _userQueries;

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

        [POST("")]
        [AllowAnonymous]
        [Transaction]
        public virtual UsersPostOkResponseContent Post(UsersPostRequestContent requestContent)
        {
            var command = new RegisterUser(
                requestContent.Email, requestContent.Password, requestContent.FirstName,
                requestContent.LastName, requestContent.Street, requestContent.Postcode,
                requestContent.City, requestContent.MobilePhone);
            Dispatcher.Dispatch(command);


            if (requestContent.OrganizationId.HasValue)
            {
                Dispatcher.Dispatch(new ApplyForMembership
                {
                    ApplicantId = command.ResultingUserId,
                    OrganizationId = requestContent.OrganizationId.Value
                });
            }

            return new UsersPostOkResponseContent {UserGuid = command.ResultingUserGuid};
        }

        [GET("{userId}")]
        [Transaction]
        public virtual UsersGetOkResponseContent Get(int userId)
        {
            var user = _userQueries.GetById(userId);
            if ((user == null) || (user.Id != CurrentUserId.Id))
                throw new HttpException((int) HttpStatusCode.NotFound, "User not found.");

            var memberships = _membershipQueries.ByUserId(user.Id);
            var store = _storeQueries.FindByOwnerId(new OwnerId(user.Guid));

            return new UsersGetOkResponseContent(user, memberships, store);
        }
    }


    public class UsersPostOkResponseContent
    {
        [JsonProperty("userGuid")]
        public Guid UserGuid { get; set; }
    }

    public class UsersPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("organizationId")]
        public Guid? OrganizationId { get; set; }
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
                    StoreId = store.StoreId.Id,
                    Address = store.Address,
                    OpeningHours = store.OpeningHours
                };

                if (store.Latitude.HasValue && store.Longitude.HasValue)
                {
                    Store.Coordinate = new Coordinate
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
}