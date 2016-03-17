namespace Phundus.Rest.Api
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/lessees")]
    public class LesseesController : ApiControllerBase
    {
        private readonly ILesseeQueryService _lesseeQueryService;

        public LesseesController(ILesseeQueryService lesseeQueryService)
        {
            _lesseeQueryService = lesseeQueryService;
        }

        [GET("{lesseeId}")]
        [Transaction]
        public virtual LesseesGetOkResponseContent Get(Guid lesseeId)
        {
            var lessee = _lesseeQueryService.GetById(CurrentUserId, lesseeId);
            return new LesseesGetOkResponseContent
            {
                LesseeId = lessee.LesseeId,
                Name = lessee.Name,
                PostalAddress = lessee.PostalAddress,
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

        [JsonProperty("postalAddress")]
        public string PostalAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}