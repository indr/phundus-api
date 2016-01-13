namespace Phundus.Rest.Api
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Integration.Shop;
    using Newtonsoft.Json;

    [RoutePrefix("api/lessees")]
    public class LesseesController : ApiControllerBase
    {
        private readonly ILesseeQueries _lesseeQueries;

        public LesseesController(ILesseeQueries lesseeQueries)
        {
            if (lesseeQueries == null) throw new ArgumentNullException("lesseeQueries");
            _lesseeQueries = lesseeQueries;
        }

        [GET("{lesseeGuid}")]
        [Transaction]
        public virtual LesseesGetOkResponseContent Get(Guid lesseeGuid)
        {
            var lessee = _lesseeQueries.GetByGuid(lesseeGuid);
            return new LesseesGetOkResponseContent
            {
                LesseeGuid = lessee.LesseeGuid,
                Name = lessee.Name,
                Address = lessee.Address,
                PhoneNumber = lessee.PhoneNumber,
                EmailAddress = lessee.EmailAddress
            };
        }
    }

    public class LesseesGetOkResponseContent
    {
        [JsonProperty("lesseeGuid")]
        public Guid LesseeGuid { get; set; }

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