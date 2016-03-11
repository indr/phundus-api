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
            var lessor = _lessorQueries.GetById(lessorId);

            return new LessorsGetOkResponseContent
            {
                LessorId = lessor.LessorId,
                Type = lessor.Type.ToString().ToLowerInvariant(),
                Name = lessor.Name,
                Url = lessor.Url,
                PostalAddress = lessor.PostalAddress,
                PhoneNumber = lessor.PhoneNumber,
                EmailAddress = lessor.EmailAddress,
                Website = lessor.Website,
                PublicRental = lessor.PublicRental
            };
        }
    }

    public class LessorsGetOkResponseContent
    {
        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("postalAddress")]
        public string PostalAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("publicRental")]
        public bool PublicRental { get; set; }
    }
}