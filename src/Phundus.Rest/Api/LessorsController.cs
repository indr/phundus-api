﻿namespace Phundus.Rest.Api
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Resources;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/lessors")]
    public class LessorsController : ApiControllerBase
    {
        private readonly ILessorQueryService _lessorQueryService;

        public LessorsController(ILessorQueryService lessorQueryService)
        {
            _lessorQueryService = lessorQueryService;
        }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual QueryOkResponseContent<LessorData> Get()
        {
            var results = _lessorQueryService.Query();

            return new QueryOkResponseContent<LessorData>(results);
        }

        [GET("{lessorId}")]
        [Transaction]
        [AllowAnonymous]
        public virtual GetOkResponseContent Get(Guid lessorId)
        {
            var lessor = _lessorQueryService.GetById(lessorId);

            return new GetOkResponseContent
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

        public class GetOkResponseContent
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
}