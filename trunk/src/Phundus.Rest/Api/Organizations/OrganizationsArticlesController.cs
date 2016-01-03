namespace Phundus.Rest.Api.Organizations
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
    using Core.Inventory.Stores.Model;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/articles")]
    public class OrganizationsArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IAvailabilityQueries _availabilityQueries;
        private readonly IMemberInRole _memberInRole;
        private readonly IReservationRepository _reservationRepository;
        private IStoreQueries _storeQueries;

        public OrganizationsArticlesController(IMemberInRole memberInRole, IStoreQueries storeQueries,
            IArticleQueries articleQueries, IAvailabilityQueries availabilityQueries,
            IReservationRepository reservationRepository)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeQueries, "StoreQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(availabilityQueries, "AvailabilityQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationRepository, "ReservationRepository must be provided.");

            _memberInRole = memberInRole;
            _storeQueries = storeQueries;
            _articleQueries = articleQueries;
            _availabilityQueries = availabilityQueries;
            _reservationRepository = reservationRepository;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid organizationId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId.Id);

            var result = _articleQueries.FindByOwnerId(organizationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{articleId}")]
        [Transaction]
        public virtual OrganizationsArticlesGetOkResponseContent Get(Guid organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId.Id);
            // TODO: Prüfen ob Artikel der Organization gehört            

            var result = _articleQueries.GetById(articleId);

            return new OrganizationsArticlesGetOkResponseContent
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
        public virtual HttpResponseMessage GetDescription(Guid organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId.Id);
            // TODO: Prüfen ob Artikel der Organization gehört  

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Description);
        }

        [GET("{articleId}/specification")]
        [Transaction]
        public virtual HttpResponseMessage GetSpecification(Guid organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId.Id);
            // TODO: Prüfen ob Artikel der Organization gehört  

            var result = _articleQueries.GetById(articleId);

            return Request.CreateResponse(HttpStatusCode.OK, result.Specification);
        }

        [GET("{articleId}/stock")]
        [Transaction]
        public virtual HttpResponseMessage GetStock(Guid organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId.Id);
            // TODO: Prüfen ob Artikel der Organization gehört  

            var availabilities = _availabilityQueries.GetAvailability(articleId).ToList();
            var reservations = _reservationRepository.Find(articleId, Guid.Empty).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new {availabilities, reservations});
        }

        [POST("")]
        [Transaction]
        public virtual OrganizationsArticlesPostOkResponseContent Post(Guid organizationId,
            OrganizationsArticlesPostRequestContent requestContent)
        {
            var ownerId = new OwnerId(organizationId);
            var storeId = _storeQueries.GetByOwnerId(ownerId).StoreId;
            var command = new CreateArticle(CurrentUserId, ownerId, storeId,
                requestContent.Name);
            Dispatch(command);

            return new OrganizationsArticlesPostOkResponseContent
            {
                ArticleId = command.ResultingArticleId
            };
        }

        [PUT("{articleId}")]
        [Transaction]
        public virtual OrganizationsArticlesPutOkResponseContent Put(Guid organizationId, int articleId,
            OrganizationsArticlesPutRequestContent requestContent)
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

            return new OrganizationsArticlesPutOkResponseContent
            {
                ArticleId = articleId
            };
        }

        [PUT("{articleId}/description")]
        [Transaction]
        public virtual HttpResponseMessage PutDescription(Guid organizationId, int articleId, dynamic requestContent)
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
        public virtual HttpResponseMessage PutSpecification(Guid organizationId, int articleId, dynamic requestContent)
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
        public virtual HttpResponseMessage Delete(Guid organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId.Id);

            Dispatcher.Dispatch(new DeleteArticle { ArticleId = articleId, InitiatorId = CurrentUserId.Id });
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class OrganizationsArticlesGetOkResponseContent
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

    public class OrganizationsArticlesPostRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class OrganizationsArticlesPostOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class OrganizationsArticlesPutOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class OrganizationsArticlesPutRequestContent
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