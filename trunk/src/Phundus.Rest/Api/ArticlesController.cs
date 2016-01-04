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
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Core.Inventory.Queries;
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

            int integer;
            if (!Int32.TryParse(ownerId, out integer))
                return null;

            var user = _userQueries.FindById(integer);
            if (user == null)
                return null;

            return new OwnerId(user.Guid);
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

            _memberInRole.ActiveChief(ownerId, CurrentUserId.Id);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt  

            var results = _articleQueries.FindByOwnerId(ownerId.Id);
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

            _memberInRole.ActiveChief(ownerId, CurrentUserId.Id);
            // TODO: Pr�fen ob Artikel dem Owner geh�rt  

            var result = _articleQueries.GetById(articleId);

            return new ArticlesGetOkResponseContent
            {
                ArticleId = result.Id,
                Name = result.Name,
                Brand = result.Brand,
                Color = result.Color,
                GrossStock = result.GrossStock,
                Price = result.Price
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

            _memberInRole.ActiveChief(ownerId, CurrentUserId.Id);
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

            _memberInRole.ActiveChief(ownerId, CurrentUserId.Id);
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

            _memberInRole.ActiveChief(ownerId, CurrentUserId.Id);
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
            var storeId = _storeQueries.GetByOwnerId(ownerId).StoreId;
            var command = new CreateArticle(CurrentUserId, ownerId, storeId,
                requestContent.Name, requestContent.Amount);
            Dispatch(command);

            return new ArticlesPostOkResponseContent
            {
                ArticleId = command.ResultingArticleId
            };
        }

        [PUT("{articleId}")]
        [Transaction]
        public virtual ArticlesPutOkResponseContent Put(int articleId, ArticlesPutRequestContent requestContent)
        {
            Dispatcher.Dispatch(new UpdateArticle
            {
                ArticleId = articleId,
                Brand = requestContent.Brand,
                Color = requestContent.Color,
                GrossStock = requestContent.GrossStock,
                InitiatorId = CurrentUserId.Id,
                Name = requestContent.Name,
                Price = requestContent.Price
            });

            return new ArticlesPutOkResponseContent
            {
                ArticleId = articleId
            };
        }

        [PUT("{articleId}/description")]
        [Transaction]
        public virtual HttpResponseMessage PutDescription(int articleId, dynamic requestContent)
        {
            Dispatcher.Dispatch(new UpdateDescription
            {
                ArticleId = articleId,
                Description = requestContent.data,
                InitiatorId = CurrentUserId.Id
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [PUT("{articleId}/specification")]
        [Transaction]
        public virtual HttpResponseMessage PutSpecification(int articleId, dynamic requestContent)
        {
            Dispatcher.Dispatch(new UpdateSpecification
            {
                ArticleId = articleId,
                Specification = requestContent.data,
                InitiatorId = CurrentUserId.Id
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [DELETE("{articleId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int articleId)
        {
            Dispatcher.Dispatch(new DeleteArticle {ArticleId = articleId, InitiatorId = CurrentUserId.Id});
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

        [JsonProperty("grossStock")]
        public int GrossStock { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    public class ArticlesPostRequestContent
    {
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class ArticlesPostOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class ArticlesPutOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class ArticlesPutRequestContent
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
    }
}