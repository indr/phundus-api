namespace Phundus.Shop.Model
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Inventory.Model.Articles;
    using Inventory.Model.Collaborators;

    public interface IArticleService
    {
        // TODO: should not return article if lessee can not rent it
        // TODO: should throw exception if lessee can not rent article
        Article GetById(LessorId lessorId, ArticleId articleId, LesseeId lesseeId);
    }

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICollaboratorService _collaboratorService;
        private readonly ILessorService _lessorService;

        public ArticleService(ICollaboratorService collaboratorService, IArticleRepository articleRepository,
            ILessorService lessorService)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
            _lessorService = lessorService;
        }

        protected ArticleService()
        {
        }

        public virtual Article GetById(LessorId lessorId, ArticleId articleId, LesseeId lesseeId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            var result = GetById(articleId, new UserId(lesseeId.Id));
            if (!Equals(result.LessorId, lessorId))
                throw new NotFoundException(String.Format("Article {0} {1} not found.", lessorId, articleId));

            return result;
        }

        private Article GetById(ArticleId articleId, UserId userId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (userId == null) throw new ArgumentNullException("userId");

            var article = _articleRepository.GetById(articleId);
            return ConvertToInternal(article, userId);
        }

        private Article ConvertToInternal(Inventory.Articles.Model.Article article, UserId userId)
        {
            var lessor = _lessorService.GetById(new LessorId(article.Owner.OwnerId.Id));
            var price = article.PublicPrice;
            if (article.MemberPrice.HasValue && article.MemberPrice.Value > 0 && IsMember(userId, lessor.LessorId))
            {
                price = article.MemberPrice.Value;
            }

            return new Article(article.ArticleShortId, article.ArticleId, lessor, article.StoreId, article.Name, price);
        }

        private bool IsMember(UserId userId, LessorId lessorId)
        {
            return _collaboratorService.IsMember(userId, lessorId);
        }
    }
}