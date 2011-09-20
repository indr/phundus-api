using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IArticleService
    {
        ArticleDto GetArticle(string sessionKey, int id);
        int CreateArticle(string sessionKey, ArticleDto subject);
        void UpdateArticle(string sessionKey, ArticleDto subject);
        PropertyDto[] GetProperties(string sessionKey);
    }
}