namespace Phundus.Rest.Api.Shop
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using ContentObjects;
    using Newtonsoft.Json;
    using Phundus.Shop.Queries;

    [RoutePrefix("api/shop/items")]
    public class ShopItemsController : ApiControllerBase
    {
        private readonly IItemQueries _itemQueries;

        public ShopItemsController(IItemQueries itemQueries)
        {
            if (itemQueries == null) throw new ArgumentNullException("itemQueries");
            _itemQueries = itemQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<ShopQueryItem> Get(Guid? lessorId, string q)
        {
            var results = _itemQueries.Query(q);

            return new QueryOkResponseContent<ShopQueryItem>(results.Select(s => new ShopQueryItem
            {
                ArticleId = s.Id,
                Name = s.Name
            }));
        }

        [GET("{itemId}")]
        [Transaction]
        public virtual ShopItemGetOkResponseContent Get(Guid itemId)
        {
            var item = _itemQueries.Get(itemId);

            return new ShopItemGetOkResponseContent
            {
                Description = item.Description,
                ItemId = item.ArticleGuid,
                MemberPrice = item.MemberPrice,
                Name = item.Name,
                PublicPrice = item.PublicPrice,
                Specification = item.Specification,
                
                Documents = item.Documents.Select(s => new ShopItemGetOkResponseContent.DocumentObject
                {
                    FileLength = s.FileLength,
                    FileName = s.FileName,
                    FileType = s.FileType
                }).ToList(),
                
                Images = item.Images.Select(s => new ShopItemGetOkResponseContent.ImageObject
                {
                    FileLength = s.FileLength,
                    FileName = s.FileName,
                    FileType = s.FileType
                }).ToList()
            };
        }
    }

    public class ShopItemGetOkResponseContent
    {
        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

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

        [JsonProperty("images")]
        public ICollection<ImageObject> Images { get; set; }

        [JsonProperty("documents")]
        public ICollection<DocumentObject> Documents { get; set; }

        public class LessorObject
        {
            [JsonProperty("lessorId")]
            public Guid LessorId { get; set; }

            [JsonProperty("lessorName")]
            public string LessorName { get; set; }

            [JsonProperty("lessorType")]
            public int LessorType { get; set; }
        }

        public class ImageObject
        {
            [JsonProperty("fileName")]
            public string FileName { get; set; }

            [JsonProperty("fileType")]
            public string FileType { get; set; }

            [JsonProperty("fileLength")]
            public long FileLength { get; set; }
        }

        public class DocumentObject
        {
            [JsonProperty("fileName")]
            public string FileName { get; set; }

            [JsonProperty("fileType")]
            public string FileType { get; set; }

            [JsonProperty("fileLength")]
            public long FileLength { get; set; }
        }
    }
}