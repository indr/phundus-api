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
    using IdentityAccess.Queries;
    using Integration.IdentityAccess;
    using Inventory.Articles.Commands;
    using Inventory.AvailabilityAndReservation.Repositories;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/articles")]
    public class ArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IAvailabilityQueries _availabilityQueries;
        private readonly IMemberInRole _memberInRole;
        private readonly IReservationRepository _reservationRepository;
        private readonly IArticleActionsQueries _articleActionsQueries;
        private readonly IStoresQueries _storesQueries;

        public ArticlesController(IMemberInRole memberInRole, IStoresQueries storesQueries,
            IArticleQueries articleQueries, IAvailabilityQueries availabilityQueries,
            IReservationRepository reservationRepository, IUserQueries userQueries, IArticleActionsQueries articleActionsQueries)
        {
            if (articleActionsQueries == null) throw new ArgumentNullException("articleActionsQueries");
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(storesQueries, "StoreQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(availabilityQueries, "AvailabilityQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationRepository, "ReservationRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _memberInRole = memberInRole;
            _storesQueries = storesQueries;
            _articleQueries = articleQueries;
            _availabilityQueries = availabilityQueries;
            _reservationRepository = reservationRepository;
            _articleActionsQueries = articleActionsQueries;
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
        public virtual QueryOkResponseContent<Inventory.Projections.ArticleData> Get()
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            _memberInRole.ActiveManager(ownerId, CurrentUserId);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt  

            string query = null;
            if (queryParams.ContainsKey("q"))
                query = queryParams["q"];

            var results = _articleQueries.Query(CurrentUserId, ownerId, query);
            return new QueryOkResponseContent<Inventory.Projections.ArticleData>
            {
                Results = results.ToList()
            };
        }

        [GET("{articleId}")]
        [Transaction]
        public virtual ArticlesGetOkResponseContent Get(int articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            _memberInRole.ActiveManager(ownerId, CurrentUserId);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt  

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
        public virtual QueryOkResponseContent<ArticleActionData> GetActions(Guid articleId)
        {
            var result = _articleActionsQueries.GetActions(articleId);
            return new QueryOkResponseContent<ArticleActionData>(result);
        }

        [GET("{articleId}/description")]
        [Transaction]
        public virtual HttpResponseMessage GetDescription(int articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            _memberInRole.ActiveManager(ownerId, CurrentUserId);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt  

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Description);
        }

        [GET("{articleId}/specification")]
        [Transaction]
        public virtual HttpResponseMessage GetSpecification(int articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            _memberInRole.ActiveManager(ownerId, CurrentUserId);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt  

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Specification);
        }

        [GET("{articleId}/stock")]
        [Transaction]
        public virtual HttpResponseMessage GetStock(int articleId)
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            _memberInRole.ActiveManager(ownerId, CurrentUserId);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt   

            var availabilities = _availabilityQueries.GetAvailability(articleId).ToList();
            var reservations = _reservationRepository.Find(articleId, Guid.Empty).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new {availabilities, reservations});
        }

        [POST("")]
        [Transaction]
        public virtual ArticlesPostOkResponseContent Post(ArticlesPostRequestContent requestContent)
        {
            var ownerId = GetOwnerId(requestContent.OwnerId);
            var storeId = _storesQueries.GetByOwnerId(ownerId).StoreId;
            var articleGuid = new ArticleId();
            var command = new CreateArticle(CurrentUserId, ownerId, new StoreId(storeId), articleGuid,
                requestContent.Name, requestContent.GrossStock, requestContent.PublicPrice, requestContent.MemberPrice);
            Dispatch(command);

            return new ArticlesPostOkResponseContent
            {
                ArticleShortId = command.ResultingArticleId,
                ArticleId = articleGuid.Id
            };
        }

        [PATCH("{articleId}")]
        [Transaction]
        public virtual ArticlesPatchOkResponseContent Patch(int articleId, ArticlesPatchRequestContent requestContent)
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

            return new ArticlesPatchOkResponseContent
            {
                ArticleId = articleId
            };
        }

        [DELETE("{articleId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int articleId)
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
        public string OwnerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
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

    public class ArticlesPatchOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
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