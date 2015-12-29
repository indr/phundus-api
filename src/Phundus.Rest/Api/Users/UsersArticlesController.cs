namespace Phundus.Rest.Api.Users
{
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.Queries;

    [RoutePrefix("api/users/{userId}/articles")]
    public class UsersArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IMemberInRole _memberInRole;
        private readonly IUserQueries _userQueries;

        public UsersArticlesController(IMemberInRole memberInRole, IArticleQueries articleQueries,
            IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _memberInRole = memberInRole;
            _articleQueries = articleQueries;
            _userQueries = userQueries;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int userId)
        {
            var user = _userQueries.GetById(userId);
            if (user.Id != CurrentUserId)
                throw new AuthorizationException();

            var result = _articleQueries.FindByOwnerId(user.Guid);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [DELETE("{articleId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int userId, int articleId)
        {
            var user = _userQueries.GetById(userId);
            if (user.Id != CurrentUserId)
                throw new AuthorizationException();

            Dispatcher.Dispatch(new DeleteArticle {ArticleId = articleId, InitiatorId = CurrentUserId});
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}