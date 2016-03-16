namespace Phundus.Rest.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Inventory.Application;
    using Inventory.Model.Reservations;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/articles")]
    public class ArticlesController : ApiControllerBase
    {
        private readonly IArticleActionsQueries _articleActionsQueries;
        private readonly IArticleQueries _articleQueries;
        private readonly IAvailabilityQueries _availabilityQueries;
        private readonly IMemberInRole _memberInRole;
        private readonly IReservationRepository _reservationRepository;
        private readonly IShortIdGeneratorService _shortIdGeneratorService;
        private readonly IStoresQueries _storesQueries;

        public ArticlesController(IMemberInRole memberInRole, IStoresQueries storesQueries,
            IArticleQueries articleQueries, IAvailabilityQueries availabilityQueries,
            IReservationRepository reservationRepository, IArticleActionsQueries articleActionsQueries,
            IShortIdGeneratorService shortIdGeneratorService)
        {
            _memberInRole = memberInRole;
            _storesQueries = storesQueries;
            _articleQueries = articleQueries;
            _availabilityQueries = availabilityQueries;
            _reservationRepository = reservationRepository;
            _articleActionsQueries = articleActionsQueries;
            _shortIdGeneratorService = shortIdGeneratorService;
        }

        private OwnerId GetOwnerId(string ownerId)
        {
            Guid guid;
            if (Guid.TryParse(ownerId, out guid))
                return new OwnerId(guid);

            return null;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<ArticleData> GetAll()
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            // TODO: Prüfen ob Artikel dem Owner gehört  

            string query = null;
            if (queryParams.ContainsKey("q"))
                query = queryParams["q"];

            var results = _articleQueries.Query(CurrentUserId, ownerId, query);
            
            return new QueryOkResponseContent<ArticleData>(results);
        }

        [GET("{articleId}")]
        [Transaction]
        public virtual ArticlesGetOkResponseContent Get(ArticleId articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            // TODO: Prüfen ob Artikel dem Owner gehört  

            var result = _articleQueries.GetById(articleId);

            return new ArticlesGetOkResponseContent
            {
                ArticleId = result.ArticleId,
                ArticleShortId = result.ArticleShortId,
                Name = result.Name,
                Brand = result.Brand,
                Color = result.Color,
                GrossStock = result.GrossStock,
                Price = result.PublicPrice,
                PublicPrice = result.PublicPrice,
                MemberPrice = result.MemberPrice,
                Description = result.Description,
                Specification = result.Specification
            };
        }

        [GET("{articleId}/actions")]
        [Transaction]
        public virtual QueryOkResponseContent<ArticleActionData> GetActions(ArticleId articleId)
        {
            var result = _articleActionsQueries.GetActions(articleId);
            return new QueryOkResponseContent<ArticleActionData>(result);
        }

        [GET("{articleId}/description")]
        [Transaction]
        public virtual HttpResponseMessage GetDescription(ArticleId articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");
            
            // TODO: Prüfen ob Artikel dem Owner gehört  

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Description);
        }

        [GET("{articleId}/specification")]
        [Transaction]
        public virtual HttpResponseMessage GetSpecification(ArticleId articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            // TODO: Prüfen ob Artikel dem Owner gehört  

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Specification);
        }

        [GET("{articleId}/stock")]
        [Transaction]
        public virtual HttpResponseMessage GetStock(ArticleId articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");
            
            // TODO: Prüfen ob Artikel dem Owner gehört   

            var availabilities = _availabilityQueries.GetAvailability(articleId).ToList();
            var reservations = _reservationRepository.Find(articleId, null).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new {availabilities, reservations});
        }

        [POST("")]
        public virtual ArticlesPostOkResponseContent Post(ArticlesPostRequestContent requestContent)
        {
            var storeId = _storesQueries.GetByOwnerId(requestContent.OwnerId).StoreId;
            var articleId = new ArticleId();
            var articleShortId = _shortIdGeneratorService.GetNext<ArticleShortId>();
            var command = new CreateArticle(CurrentUserId, new OwnerId(requestContent.OwnerId), new StoreId(storeId),
                articleId, articleShortId,
                requestContent.Name, requestContent.GrossStock, requestContent.PublicPrice, requestContent.MemberPrice);
            Dispatch(command);

            return new ArticlesPostOkResponseContent
            {
                ArticleId = articleId.Id,
                ArticleShortId = articleShortId.Id
            };
        }

        [PATCH("{articleId}")]
        public virtual HttpResponseMessage Patch(ArticleId articleId, ArticlesPatchRequestContent requestContent)
        {
            if (!String.IsNullOrWhiteSpace(requestContent.Name))
            {
                Dispatch(new UpdateArticle(CurrentUserId, articleId, requestContent.Name, requestContent.Brand,
                    requestContent.Color, requestContent.GrossStock));
            }
            if (requestContent.Prices != null)
            {
                Dispatch(new ChangePrices(CurrentUserId, articleId, requestContent.Prices.PublicPrice,
                    requestContent.Prices.MemberPrice));
            }
            if (requestContent.Description != null)
            {
                Dispatch(new UpdateDescription(CurrentUserId, articleId, requestContent.Description));
            }
            if (requestContent.Specification != null)
            {
                Dispatch(new UpdateSpecification(CurrentUserId, articleId, requestContent.Specification));
            }

            return NoContent();
        }

        [DELETE("{articleId}")]
        public virtual HttpResponseMessage Delete(ArticleId articleId)
        {
            Dispatcher.Dispatch(new DeleteArticle(CurrentUserId, articleId));

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class ArticlesGetOkResponseContent
    {
        [JsonProperty("articleId")]
        public Guid ArticleId { get; set; }

        [JsonProperty("articleShortId")]
        public int ArticleShortId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("publicPrice")]
        public decimal PublicPrice { get; set; }

        [JsonProperty("memberPrice")]
        public decimal? MemberPrice { get; set; }

        [JsonProperty("grossStock")]
        public int GrossStock { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("specification")]
        public string Specification { get; set; }
    }

    public class ArticlesPostRequestContent
    {
        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int GrossStock { get; set; }

        [JsonProperty("publicPrice")]
        public decimal PublicPrice { get; set; }

        [JsonProperty("memberPrice")]
        public decimal? MemberPrice { get; set; }
    }

    public class ArticlesPostOkResponseContent
    {
        [JsonProperty("articleId")]
        public Guid ArticleId { get; set; }

        [JsonProperty("articleShortId")]
        public int ArticleShortId { get; set; }
    }

    public class ArticlesPatchRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("grossStock")]
        public int GrossStock { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("specification")]
        public string Specification { get; set; }

        [JsonProperty("prices")]
        public Prices Prices { get; set; }
    }

    public class Prices
    {
        [JsonProperty("publicPrice")]
        public decimal PublicPrice { get; set; }

        [JsonProperty("memberPrice")]
        public decimal? MemberPrice { get; set; }
    }
}