using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IArticleService
    {
        ArticleDto GetArticle(string sessionKey, int id);
    }
}