namespace Phundus.Rest.Api
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Newtonsoft.Json;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/lessees")]
    public class LesseesController : ApiControllerBase
    {
        private readonly ILesseeQueries _lesseeQueries;

        public LesseesController(ILesseeQueries lesseeQueries)
        {            
            _lesseeQueries = lesseeQueries;
        }

        [GET("{lesseeId}")]
        [Transaction]
        public virtual LesseesGetOkResponseContent Get(Guid lesseeId)
        {
            var lessee = _lesseeQueries.GetById(lesseeId);
            return new LesseesGetOkResponseContent
            {
                LesseeId = lessee.LesseeId,
                Name = lessee.Name,
                Address = lessee.Address,
                PhoneNumber = lessee.PhoneNumber,
                EmailAddress = lessee.EmailAddress
            };
        }
    }

    public class LesseesGetOkResponseContent
    {
        [JsonProperty("lesseeId")]
        public Guid LesseeId { get; set; }

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