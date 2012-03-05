using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IArticleService
    {
        ArticleDto GetArticle(string sessionKey, int id);
        int CreateArticle(string sessionKey, ArticleDto subject);
        void UpdateArticle(string sessionKey, ArticleDto subject);
        IList<FieldDefinitionDto> GetProperties(string sessionKey);
        ArticleDto[] GetArticles(string sessionKey);
        void DeleteArticle(string sessionKey, ArticleDto subject);

        void AddImage(string sessionKey, int articleId, ImageDto subject);
        void DeleteImage(string sessionKey, int articleId, string imageName);
        IList<ImageDto> GetImages(string sessionKey, int articleId);
        IList<ArticleDto> FindArticles(string sessionKey, string query);
    }
}