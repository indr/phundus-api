namespace Phundus.Core.Inventory.Articles
{
    using System;

    public class ArticleNotFoundException : Exception
    {
        public ArticleNotFoundException(int articleId) : base(String.Format("Der Artikel mit der Id {0} konnte nicht gefunden werden.", articleId))
        {
            
        }
    }
}