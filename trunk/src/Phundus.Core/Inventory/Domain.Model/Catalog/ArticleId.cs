namespace Phundus.Core.Inventory.Domain.Model.Catalog
{
    using Common.Domain.Model;

    public class ArticleId : Identity<int>
    {
        public ArticleId(int id) : base(id)
        {
        }
    }
}