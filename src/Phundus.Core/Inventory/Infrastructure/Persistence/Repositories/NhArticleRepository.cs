namespace Phundus.Inventory.Infrastructure.Persistence.Repositories
{
    using Articles.Model;
    using Common;
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Model.Articles;

    public class NhArticleRepository : NhRepositoryBase<Article>, IArticleRepository
    {
        public Article GetById(ArticleId articleId)
        {
            Article result = FindById(articleId);
            if (result == null)
                throw new NotFoundException("Article {0} not found.", articleId);
            return result;
        }

        public Article FindById(ArticleId articleId)
        {
            return Session.QueryOver<Article>().Where(p => p.ArticleId.Id == articleId.Id).SingleOrDefault();
        }
    }
}