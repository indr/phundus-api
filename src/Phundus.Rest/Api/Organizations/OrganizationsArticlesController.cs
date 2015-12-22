namespace Phundus.Rest.Api.Organizations
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

    [RoutePrefix("api/organizations/{organizationId}/articles")]
    public class OrganizationsArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IMemberInRole _memberInRole;

        public OrganizationsArticlesController(IMemberInRole memberInRole, IArticleQueries articleQueries)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "Member in role must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleQueries, "Article queries must be provided.");

            _memberInRole = memberInRole;
            _articleQueries = articleQueries;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId);

            var result = _articleQueries.GetArticles(organizationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [DELETE("{articleId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId);

            Dispatcher.Dispatch(new DeleteArticle { ArticleId = articleId, InitiatorId = CurrentUserId });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}