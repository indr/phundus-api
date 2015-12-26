namespace Phundus.Core.Shop.Orders.Model
{
    using Inventory.Articles.Model;

    public class ArticleWrapper
    {
        private readonly Article _article;

        public ArticleWrapper(Article article)
        {
            _article = article;
        }

        public int Id { get { return _article.Id; } }
        public string Caption { get { return _article.Caption; } }
        public decimal Price { get { return _article.Price; } }
        public int OrganizationId { get { return _article.OrganizationId; } }
    }
}