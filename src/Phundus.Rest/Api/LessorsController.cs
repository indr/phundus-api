namespace Phundus.Rest.Api
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using ContentObjects;
    using Integration.Shop;
    using Newtonsoft.Json;


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
        public virtual QueryOkResponseContent<ILessor> Get()
        {
            var results = _lessorQueries.Query();
            return new QueryOkResponseContent<ILessor>(results);
        }

        [GET("{lessorGuid}")]
        [Transaction]
        public virtual LessorsGetOkResponseContent Get(Guid lessorGuid)
        {
            var lessor = _lessorQueries.GetByGuid(lessorGuid);
            return new LessorsGetOkResponseContent
            {
                LessorGuid = lessor.LessorGuid,
                LessorType = lessor.LessorType,
                Name = lessor.Name,
                Address = lessor.Address,
                PhoneNumber = lessor.PhoneNumber,
                EmailAddress = lessor.EmailAddress
            };
        }
    }

    public class LessorsGetOkResponseContent
    {
        [JsonProperty("lessorGuid")]
        public Guid LessorGuid { get; set; }

        [JsonProperty("lessorType")]
        public int LessorType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}