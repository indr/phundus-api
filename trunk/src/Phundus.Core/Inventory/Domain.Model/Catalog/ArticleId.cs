namespace Phundus.Core.Inventory.Domain.Model.Catalog
{
    using Common;
    using Common.Domain.Model;

    public class ArticleId : Identity<int>
    {
        public ArticleId(int id) : base(id)
        {
            AssertionConcern.AssertArgumentGreaterThan(id, 0, "Article id must be greater than zero.");
        }
    }
}