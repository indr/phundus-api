namespace Phundus.Core.Inventory.Domain.Model.Articles
{
    using Common.Domain.Model;

    public class ArticleId : Identity<int>
    {
        public ArticleId(int id) : base(id)
        {
        }
    }
}