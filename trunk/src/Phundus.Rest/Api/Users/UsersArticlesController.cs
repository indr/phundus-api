namespace Phundus.Rest.Api.Users
{
    using System;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/users/{userId}/articles")]
    public class UsersArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IUserQueries _userQueries;

        public UsersArticlesController(IArticleQueries articleQueries,
            IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _articleQueries = articleQueries;
            _userQueries = userQueries;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int userId)
        {
            var currentUserGuid = EnforceCurrentUser(userId);

            var result = _articleQueries.FindByOwnerId(currentUserGuid);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        [POST("")]
        [Transaction]
        public virtual UsersArticlesOkResponseContent Post(int userId, UsersArticlesPostRequestContent requestContent)
        {
            throw new NotImplementedException();
        }

        [DELETE("{articleId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int userId, int articleId)
        {
            EnforceCurrentUser(userId);

            Dispatcher.Dispatch(new DeleteArticle {ArticleId = articleId, InitiatorId = CurrentUserId});
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private Guid EnforceCurrentUser(int userId)
        {
            var user = _userQueries.GetById(userId);
            if (user.Id != CurrentUserId)
                throw new AuthorizationException();
            return user.Guid;
        }
    }

    public class UsersArticlesPostRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class UsersArticlesOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }
}