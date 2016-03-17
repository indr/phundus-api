namespace Phundus.Rest.Api.Shop
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Common.Resources;
    using ContentObjects;
    using Inventory.Application;
    using Inventory.Projections;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/shop/items")]
    [AllowAnonymous]
    public class ShopItemsController : ApiControllerBase
    {
        private readonly IAvailabilityQueryService _availabilityQueryService;
        private readonly IProductQueryService _productQueryService;

        public ShopItemsController(IProductQueryService productQueryService,
            IAvailabilityQueryService availabilityQueryService)
        {
            _productQueryService = productQueryService;
            _availabilityQueryService = availabilityQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<ShopQueryItem> Get([FromUri] ShopItemsQueryRequestContent requestContent)
        {
            var results = _productQueryService.Query(requestContent.Q, requestContent.LessorId, requestContent.Offset,
                requestContent.Limit);

            return QueryOkResponseContent<ShopQueryItem>.Build(results, s => new ShopQueryItem
            {
                ItemId = s.ArticleId,
                ItemShortId = s.ArticleShortId,
                Name = s.Name,
                LessorId = s.LessorId,
                LessorName = s.LessorName,
                LessorType = s.LessorType.ToLowerString(),
                StoreId = s.StoreId,
                StoreName = s.StoreName,
                MemberPrice = s.MemberPrice,
                PreviewImageUrl = GetArticleFileUrl(s.ArticleId, Path.GetFileName(s.PreviewImageFileName)),
                PublicPrice = s.PublicPrice
            });
        }

        [GET("{itemId}")]
        [Transaction]
        public virtual ShopItemGetOkResponseContent Get(Guid itemId)
        {
            var item = _productQueryService.Get(itemId);

            return new ShopItemGetOkResponseContent
            {
                Description = item.Description,
                ItemId = item.ArticleId,
                ItemShortId = item.ArticleShortId,
                MemberPrice = item.MemberPrice,
                Name = item.Name,
                PublicPrice = item.PublicPrice,
                Specification = item.Specification,
                Lessor = new ShopItemGetOkResponseContent.LessorObject
                {
                    LessorId = item.LessorId,
                    Type = item.LessorType.ToLowerString(),
                    Name = item.LessorName,
                    Url = item.LessorUrl
                },
                Store = new ShopItemGetOkResponseContent.StoreObject
                {
                    StoreId = item.StoreId,
                    Name = item.StoreName
                },
                Documents = item.Documents.Select(s => new ShopItemGetOkResponseContent.DocumentObject
                {
                    FileLength = s.FileLength,
                    FileName = s.FileName,
                    FileType = s.FileType,
                    Url = GetArticleFileUrl(item.ArticleId, s.FileName)
                }).ToList(),
                Images = item.Images.Select(s => new ShopItemGetOkResponseContent.ImageObject
                {
                    FileLength = s.FileLength,
                    FileName = s.FileName,
                    FileType = s.FileType,
                    Url = GetArticleFileUrl(item.ArticleId, s.FileName)
                }).ToList()
            };
        }

        private string GetArticleFileUrl(Guid articleId, string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                return null;
            const string format = @"/Content/Images/Articles/{0}/{1}";
            return String.Format(format, articleId.ToString("D"), fileName);
        }

        [GET("{articleId}/availability")]
        [Transaction]
        public virtual HttpResponseMessage GetAvailability(ArticleId articleId)
        {
            var result = _availabilityQueryService.GetAvailability(articleId);
            return Ok(new {result});
        }
    }

    public class ShopItemsQueryRequestContent
    {
        [JsonProperty("lessorId")]
        public Guid? LessorId { get; set; }

        [JsonProperty("q")]
        public string Q { get; set; }

        [JsonProperty("offset")]
        public int? Offset { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

    public class ShopItemGetOkResponseContent
    {
        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("itemShortId")]
        public int ItemShortId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("publicPrice")]
        public decimal PublicPrice { get; set; }

        [JsonProperty("memberPrice")]
        public decimal? MemberPrice { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("specification")]
        public string Specification { get; set; }

        [JsonProperty("lessor")]
        public LessorObject Lessor { get; set; }

        [JsonProperty("store")]
        public StoreObject Store { get; set; }

        [JsonProperty("images")]
        public ICollection<ImageObject> Images { get; set; }

        [JsonProperty("documents")]
        public ICollection<DocumentObject> Documents { get; set; }

        public class DocumentObject
        {
            [JsonProperty("fileName")]
            public string FileName { get; set; }

            [JsonProperty("fileType")]
            public string FileType { get; set; }

            [JsonProperty("fileLength")]
            public long FileLength { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class ImageObject
        {
            [JsonProperty("fileName")]
            public string FileName { get; set; }

            [JsonProperty("fileType")]
            public string FileType { get; set; }

            [JsonProperty("fileLength")]
            public long FileLength { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class LessorObject
        {
            [JsonProperty("lessorId")]
            public Guid LessorId { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class StoreObject
        {
            [JsonProperty("storeId")]
            public Guid StoreId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}