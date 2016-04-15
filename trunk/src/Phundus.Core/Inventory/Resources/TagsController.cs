namespace Phundus.Inventory.Resources
{
    using System.Web.Http;
    using Application;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Resources;
    using Projections;

    [RoutePrefix("api/inventory/tags")]
    public class TagsController : ApiControllerBase
    {
        private readonly ITagsQueryService _tagsQueryService;

        public TagsController(ITagsQueryService tagsQueryService)
        {
            _tagsQueryService = tagsQueryService;
        }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual QueryOkResponseContent<TagData> GetAll()
        {
            string q = null;
            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("q"))
                q = queryParams["q"];
            var results = _tagsQueryService.Query(q);
            return new QueryOkResponseContent<TagData>(results);
        }
    }
}