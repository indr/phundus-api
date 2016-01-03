namespace Phundus.Rest.Api.Users
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

    [RoutePrefix("api/users/{userId}/articles")]
    public class UsersArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IAvailabilityQueries _availabilityQueries;
        private readonly IReservationRepository _reservationRepository;
        private readonly IStoreQueries _storeQueries;
        private readonly IUserQueries _userQueries;

        public UsersArticlesController(IArticleQueries articleQueries, IStoreQueries storeQueries,
            IUserQueries userQueries, IAvailabilityQueries availabilityQueries,
            IReservationRepository reservationRepository)
        {
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeQueries, "StoreQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(availabilityQueries, "AvailabilityQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationRepository, "ReservationRepository must be provided.");

            _articleQueries = articleQueries;
            _storeQueries = storeQueries;
            _userQueries = userQueries;
            _availabilityQueries = availabilityQueries;
            _reservationRepository = reservationRepository;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int userId)
        {
            var currentUserGuid = EnforceCurrentUser(userId);

            var result = _articleQueries.FindByOwnerId(currentUserGuid);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{articleId}")]
        [Transaction]
        public virtual UsersArticlesGetOkResponseContent Get(int userId, int articleId)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

            var result = _articleQueries.GetById(articleId);

            return new UsersArticlesGetOkResponseContent
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
        public virtual HttpResponseMessage GetDescription(int userId, int articleId)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Description);
        }

        [GET("{articleId}/specification")]
        [Transaction]
        public virtual HttpResponseMessage GetSpecification(int userId, int articleId)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Specification);
        }

        [GET("{articleId}/stock")]
        [Transaction]
        public virtual HttpResponseMessage GetStock(int userId, int articleId)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

            var availabilities = _availabilityQueries.GetAvailability(articleId).ToList();
            var reservations = _reservationRepository.Find(articleId, Guid.Empty).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new {availabilities, reservations});
        }

        [POST("")]
        [Transaction]
        public virtual UsersArticlesPostOkResponseContent Post(int userId,
            UsersArticlesPostRequestContent requestContent)
        {
            var currentUserGuid = EnforceCurrentUser(userId);

            var ownerId = new OwnerId(currentUserGuid);
            var storeId = _storeQueries.GetByOwnerId(ownerId).StoreId;
            var command = new CreateArticle(CurrentUserId, ownerId, storeId,
                requestContent.Name, requestContent.Amount);
            Dispatch(command);

            return new UsersArticlesPostOkResponseContent
            {
                ArticleId = command.ResultingArticleId
            };
        }

        [PUT("{articleId}")]
        [Transaction]
        public virtual UsersArticlesPutOkResponseContent Put(int userId, int articleId,
            UseresArticlesPutRequestContent requestContent)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

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

            return new UsersArticlesPutOkResponseContent
            {
                ArticleId = articleId
            };
        }

        [PUT("{articleId}/description")]
        [Transaction]
        public virtual HttpResponseMessage PutDescription(int userId, int articleId, dynamic requestContent)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

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
        public virtual HttpResponseMessage PutSpecification(int userId, int articleId, dynamic requestContent)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

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
        public virtual HttpResponseMessage Delete(int userId, int articleId)
        {
            // TODO: Prüfen ob Artikel dem Benutzer gehört
            EnforceCurrentUser(userId);

            Dispatcher.Dispatch(new DeleteArticle { ArticleId = articleId, InitiatorId = CurrentUserId.Id });
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private Guid EnforceCurrentUser(int userId)
        {
            var user = _userQueries.GetById(userId);
            if (user.Id != CurrentUserId.Id)
                throw new AuthorizationException();
            return user.Guid;
        }
    }

    public class UsersArticlesGetOkResponseContent
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

    public class UsersArticlesPostRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class UsersArticlesPostOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class UsersArticlesPutOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class UseresArticlesPutRequestContent
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