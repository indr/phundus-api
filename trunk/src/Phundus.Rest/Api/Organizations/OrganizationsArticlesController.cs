namespace Phundus.Rest.Api.Organizations
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

    [RoutePrefix("api/organizations/{organizationId}/articles")]
    public class OrganizationsArticlesController : ApiControllerBase
    {
        private readonly IArticleQueries _articleQueries;
        private readonly IMemberInRole _memberInRole;

        public OrganizationsArticlesController(IMemberInRole memberInRole, IArticleQueries articleQueries)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleQueries, "ArticleQueries must be provided.");

            _memberInRole = memberInRole;
            _articleQueries = articleQueries;
        }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid organizationId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId);

            var result = _articleQueries.FindByOwnerId(organizationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [DELETE("{articleId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, int articleId)
        {
            _memberInRole.ActiveChief(organizationId, CurrentUserId);

            Dispatcher.Dispatch(new DeleteArticle { ArticleId = articleId, InitiatorId = CurrentUserId });
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}