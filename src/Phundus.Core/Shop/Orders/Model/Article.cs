namespace Phundus.Core.Shop.Orders.Model
{
    public class Article
    {
        private readonly Inventory.Articles.Model.Article _article;

        public Article(Inventory.Articles.Model.Article article)
        {
            _article = article;
        }

        public int Id
        {
            get { return _article.Id; }
        }

        public string Caption
        {
            get { return _article.Caption; }
        }

        public decimal Price
        {
            get { return _article.Price; }
        }

        public int OrganizationId
        {
            get { return _article.OrganizationId; }
        }
    }
}