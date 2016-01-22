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
    using Inventory.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/articles")]
    public class ArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IAvailabilityQueries _availabilityQueries;
        private readonly IMemberInRole _memberInRole;
        private readonly IReservationRepository _reservationRepository;
        private readonly IStoreQueries _storeQueries;
        private readonly IUserQueries _userQueries;

        public ArticlesController(IMemberInRole memberInRole, IStoreQueries storeQueries,
            IArticleQueries articleQueries, IAvailabilityQueries availabilityQueries,
            IReservationRepository reservationRepository, IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeQueries, "StoreQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(availabilityQueries, "AvailabilityQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationRepository, "ReservationRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _memberInRole = memberInRole;
            _storeQueries = storeQueries;
            _articleQueries = articleQueries;
            _availabilityQueries = availabilityQueries;
            _reservationRepository = reservationRepository;
            _userQueries = userQueries;
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
        public virtual QueryOkResponseContent<ArticleDto> Get()
        {
            var ownerId = (OwnerId) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("ownerId"))
                ownerId = GetOwnerId(queryParams["ownerId"]);

            if (ownerId == null)
                throw new NotFoundException("Article not found.");

            _memberInRole.ActiveManager(ownerId, CurrentUserId);
            // TODO: Prüfen ob Artikel dem Owner gehört  

            string query = null;
            if (queryParams.ContainsKey("q"))
                query = queryParams["q"];

            var results = _articleQueries.Query(CurrentUserId, ownerId, query);
            return new QueryOkResponseContent<ArticleDto>
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
            // TODO: Prüfen ob Artikel dem Owner gehört  

            var result = _articleQueries.GetById(articleId);

            return new ArticlesGetOkResponseContent
            {
                ArticleId = result.Id,
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
            // TODO: Prüfen ob Artikel dem Owner gehört  

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
            // TODO: Prüfen ob Artikel dem Owner gehört  

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
            // TODO: Prüfen ob Artikel dem Owner gehört   

            var availabilities = _availabilityQueries.GetAvailability(articleId).ToList();
            var reservations = _reservationRepository.Find(articleId, Guid.Empty).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new {availabilities, reservations});
        }

        [POST("")]
        [Transaction]
        public virtual ArticlesPostOkResponseContent Post(ArticlesPostRequestContent requestContent)
        {
            var ownerId = GetOwnerId(requestContent.OwnerId);
            var storeId = _storeQueries.GetByOwnerId(ownerId).StoreId;
            var articleGuid = new ArticleGuid();
            var command = new CreateArticle(CurrentUserId, ownerId, storeId, articleGuid,
                requestContent.Name, requestContent.GrossStock, requestContent.PublicPrice, requestContent.MemberPrice);
            Dispatch(command);

            return new ArticlesPostOkResponseContent
            {
                ArticleId = command.ResultingArticleId
            };
        }

        [PATCH("{articleId}")]
        [Transaction]
        public virtual ArticlesPatchOkResponseContent Patch(int articleId, ArticlesPatchRequestContent requestContent)
        {
            if (!String.IsNullOrWhiteSpace(requestContent.Name))
            {
                Dispatcher.Dispatch(new UpdateArticle
                {
                    ArticleId = articleId,
                    Brand = requestContent.Brand,
                    Color = requestContent.Color,
                    GrossStock = requestContent.GrossStock,
                    InitiatorId = CurrentUserId,
                    Name = requestContent.Name,
                    Price = requestContent.Price
                });
            }
            if (requestContent.Description != null)
            {
                Dispatcher.Dispatch(new UpdateDescription
                {
                    ArticleId = articleId,
                    Description = requestContent.Description,
                    InitiatorId = CurrentUserId
                });
            }
            if (requestContent.Specification != null)
            {
                Dispatcher.Dispatch(new UpdateSpecification
                {
                    ArticleId = articleId,
                    Specification = requestContent.Specification,
                    InitiatorId = CurrentUserId
                });
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
            Dispatcher.Dispatch(new DeleteArticle {ArticleId = articleId, InitiatorId = CurrentUserId});
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class ArticlesGetOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

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
        public int ArticleId { get; set; }
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

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("grossStock")]
        public int GrossStock { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("specification")]
        public string Specification { get; set; }
    }
}