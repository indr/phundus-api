using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Paging;

namespace phiNdus.fundus.Business.SecuredServices
{
    public interface IArticleService
    {
        ArticleDto GetArticle(string sessionKey, int id);
        int CreateArticle(string sessionKey, ArticleDto subject);
        void UpdateArticle(string sessionKey, ArticleDto subject);
        IList<FieldDefinitionDto> GetProperties(string sessionKey);
        ArticleDto[] GetArticles(string sessionKey);
        void DeleteArticle(string sessionKey, ArticleDto subject);


        IList<AvailabilityDto> GetAvailability(string sessionKey, int id);

        void AddImage(string sessionKey, int articleId, ImageDto subject);
        void DeleteImage(string sessionKey, int articleId, string imageName);
        IList<ImageDto> GetImages(string sessionKey, int articleId);

        PagedResult<ArticleDto> FindArticles(string sessionKey, PageRequest pageRequest, string query);
    }
}