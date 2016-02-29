namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using ContentObjects;
    using Newtonsoft.Json;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/lessors")]
    public class LessorsController : ApiControllerBase
    {
        private readonly ILessorQueries _lessorQueries;

        public LessorsController(ILessorQueries lessorQueries)
        {
            if (lessorQueries == null) throw new ArgumentNullException("lessorQueries");

            _lessorQueries = lessorQueries;
        }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual QueryOkResponseContent<LessorData> Get()
        {
            var results = _lessorQueries.Query();
            return new QueryOkResponseContent<LessorData>(results);
        }

        [GET("{lessorId}")]
        [Transaction]
        [AllowAnonymous]
        public virtual LessorsGetOkResponseContent Get(Guid lessorId)
        {
            var lessor = _lessorQueries.GetByGuid(lessorId);
            return new LessorsGetOkResponseContent
            {
                LessorId = lessor.LessorId,
                LessorType = lessor.Type.ToString().ToLowerInvariant(),
                Name = lessor.Name,
                Address = lessor.PostalAddress,
                PhoneNumber = lessor.PhoneNumber,
                EmailAddress = lessor.EmailAddress,
                PublicRental = lessor.PublicRental
            };
        }
    }

    public class LessorsGetOkResponseContent
    {
        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }

        [JsonProperty("lessorType")]
        public string LessorType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("publicRental")]
        public bool PublicRental { get; set; }
    }
}