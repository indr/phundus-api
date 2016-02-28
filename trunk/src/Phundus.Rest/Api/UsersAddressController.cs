namespace Phundus.Rest.Api
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/users/{userId}/address")]
    public class UsersAddressController : ApiControllerBase
    {
        private readonly IUserAddressQueries _userAddressQueries;

        public UsersAddressController(IUserAddressQueries userAddressQueries)
        {
            if (userAddressQueries == null) throw new ArgumentNullException("userAddressQueries");

            _userAddressQueries = userAddressQueries;
        }

        [GET("")]
        [Transaction]
        public virtual UsersAddressGetOkResponseContent GetAddress(Guid userId)
        {
            var result = _userAddressQueries.GetById(CurrentUserId, userId);
            return new UsersAddressGetOkResponseContent
            {
                City = result.City,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                Postcode = result.Postcode,
                Street = result.Street,
                UserId = result.UserId
            };
        }

        [PUT("")]
        [Transaction]
        public virtual HttpResponseMessage PutAddress(Guid userId, UsersAddressPutRequestContent requestContent)
        {
            Dispatch(new ChangeUserAddress(CurrentUserId, new UserId(userId), requestContent.FirstName,
                requestContent.LastName, requestContent.Street, requestContent.Postcode, requestContent.City,
                requestContent.PhoneNumber));
            return NoContent();
        }
    }

    public class UsersAddressGetOkResponseContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

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
}