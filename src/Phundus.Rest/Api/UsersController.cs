namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMembershipQueries _membershipQueries;
        private readonly IStoresQueries _storesQueries;
        private readonly IUserAddressQueries _userAddressQueries;
        private readonly IUsersQueries _usersQueries;

        public UsersController(IUsersQueries usersQueries, IMembershipQueries membershipQueries,
            IStoresQueries storesQueries, IUserAddressQueries userAddressQueries)
        {            
            _usersQueries = usersQueries;
            _membershipQueries = membershipQueries;
            _storesQueries = storesQueries;
            _userAddressQueries = userAddressQueries;
        }

        [GET("{userId}")]
        [AllowAnonymous]
        [Transaction]
        public virtual UsersGetOkResponseContent Get(Guid userId)
        {
            var user = _usersQueries.GetById(userId);
            
            var memberships = _membershipQueries.FindByUserId(userId);
            var store = _storesQueries.FindByOwnerId(userId);
            var address = _userAddressQueries.FindById(CurrentUserIdOrNull, userId);

            var result = new UsersGetOkResponseContent(user, memberships, address);
            result.Store = Map<StoreDetailsCto>(store);
            return result;
        }

        [POST("")]
        [AllowAnonymous]        
        public virtual UsersPostOkResponseContent Post(UsersPostRequestContent requestContent)
        {
            CheckForMaintenanceMode(requestContent.Email);

            var userId = new UserId();

            Dispatch(new SignUpUser(userId, requestContent.Email, requestContent.Password,
                requestContent.FirstName, requestContent.LastName, requestContent.Street, requestContent.Postcode,
                requestContent.City, requestContent.MobilePhone));

            if (requestContent.OrganizationId.HasValue)
            {
                Dispatch(new ApplyForMembership(new InitiatorId(userId), new MembershipApplicationId(),
                    userId, new OrganizationId(requestContent.OrganizationId.Value)));
            }

            return new UsersPostOkResponseContent {UserId = userId.Id};
        }

        [PUT("{userId}/address")]        
        public virtual HttpResponseMessage PutAddress(Guid userId, UsersAddressPutRequestContent requestContent)
        {
            Dispatch(new ChangeUserAddress(CurrentUserId, new UserId(userId), requestContent.FirstName,
                requestContent.LastName, requestContent.Street, requestContent.Postcode, requestContent.City,
                requestContent.PhoneNumber));

            return NoContent();
        }
    }

    public class UsersGetOkResponseContent
    {
        public UsersGetOkResponseContent()
        {
        }

        public UsersGetOkResponseContent(IUser user, IEnumerable<MembershipData> memberships, UserAddressData address)
        {
            UserId = user.UserId;
            Username = user.EmailAddress;
            FullName = user.FirstName + " " + user.LastName;
            EmailAddress = user.EmailAddress;

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

            if (address != null)
            {
                Address = new UserAddressContentObject
                {
                    City = address.City,
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    PhoneNumber = address.PhoneNumber,
                    Postcode = address.Postcode,
                    Street = address.Street
                };
            }
        }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        [JsonProperty("memberships")]
        public List<Memberships> Memberships { get; set; }

        [JsonProperty("store")]
        public StoreDetailsCto Store { get; set; }

        [JsonProperty("address")]
        public UserAddressContentObject Address { get; set; }
    }

    public class UsersPostOkResponseContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
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

    public class UsersAddressPutRequestContent
    {
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

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class UserAddressContentObject
    {
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

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}