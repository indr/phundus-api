namespace Phundus.Persistence.Inventory.Repositories
{
    using Common;
    using Common.Domain.Model;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Articles.Repositories;

    public class NhArticleRepository : NhRepositoryBase<Article>, IArticleRepository
    {
        public new int Add(Article entity)
        {
            base.Add(entity);
            return entity.Id;
        }

        public Article GetById(ArticleId articleId)
        {
            var result = FindById(articleId);
            if (result == null)
                throw new NotFoundException("Article {0} not found.", articleId);
            return result;
        }

        public Article GetById(ArticleShortId articleShortId)
        {
            var result = FindById(articleShortId);
            if (result == null)
                throw new NotFoundException("Article {0} not found.", articleShortId);
            return result;
        }

        public Article FindById(ArticleId articleId)
        {
            return Session.QueryOver<Article>().Where(p => p.ArticleId.Id == articleId.Id).SingleOrDefault();
        }

        public Article FindById(ArticleShortId articleShortId)
        {
            return FindById(articleShortId.Id);
        }
    }
}