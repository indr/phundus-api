namespace Phundus.Shop.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using AutoMapper;
    using Common.Domain.Model;
    using Common.Resources;
    using Inventory.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/shop/products/{productId}/availability-check")]
    public class ProductsAvailabilityCheckController : ApiControllerBase
    {
        private readonly IAvailabilityQueryService _availabilityQueryService;

        static ProductsAvailabilityCheckController()
        {
            Mapper.CreateMap<IList<AvailabilitiyInfo>, PostOkResponseContent>()
                    .ForMember(d => d.IsAvailable, mo => mo.MapFrom(s => s.All(p => p.IsAvailable)))
                    .ForMember(d => d.Items, mo => mo.MapFrom(s => s));

            Mapper.CreateMap<AvailabilitiyInfo, PostOkResponseContent.Item>()
                .ForMember(d => d.FromUtc, mo => mo.MapFrom(s => s.Period.FromUtc))
                .ForMember(d => d.ToUtc, mo => mo.MapFrom(s => s.Period.ToUtc));
        }

        public ProductsAvailabilityCheckController(IAvailabilityQueryService availabilityQueryService)
        {
            _availabilityQueryService = availabilityQueryService;
        }

        [POST("")]
        [AllowAnonymous]
        public virtual PostOkResponseContent Post(ArticleId productId, PostRequestContent rq)
        {
            var quantityPeriods = rq.Items.Select(s => new QuantityPeriod(s.FromUtc, s.ToUtc, s.Quantity, s.CorrelationId)).ToArray();

            var availabilityInfo = _availabilityQueryService.IsAvailable(productId, quantityPeriods);

            return Map<PostOkResponseContent>(availabilityInfo);
        }

        public class PostRequestContent
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }

            public class Item
            {
                [JsonProperty("correlationId")]
                public Guid CorrelationId { get; set; }

                [JsonProperty("fromUtc")]
                public DateTime FromUtc { get; set; }

                [JsonProperty("toUtc")]
                public DateTime ToUtc { get; set; }

                [JsonProperty("quantity")]
                public int Quantity { get; set; }
            } 
        }

        public class PostOkResponseContent
        {
            [JsonProperty("isAvailable")]
            public bool IsAvailable { get; set; }

            [JsonProperty("items")]
            public Item[] Items { get; set; }

            public class Item
            {
                [JsonProperty("correlationId")]
                public Guid CorrelationId { get; set; }

                [JsonProperty("fromUtc")]
                public DateTime FromUtc { get; set; }

                [JsonProperty("toUtc")]
                public DateTime ToUtc { get; set; }

                [JsonProperty("quantity")]
                public int Quantity { get; set; }

                [JsonProperty("isAvailable")]
                public bool IsAvailable { get; set; }
            } 
        }
    }
}