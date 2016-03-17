namespace Phundus.Shop.Model
{
    using Common.Domain.Model;

    public interface IArticleService
    {
        // TODO: should not return article if lessee can not rent it
        // TODO: should throw exception if lessee can not rent article
        Article GetById(LessorId lessorId, ArticleId articleId, LesseeId lesseeId);
    }
}