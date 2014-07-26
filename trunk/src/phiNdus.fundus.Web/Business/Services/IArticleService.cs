namespace phiNdus.fundus.Web.Business.Services
{
    using System.Collections.Generic;
    using Dto;
    using phiNdus.fundus.Business.Paging;

    public interface IArticleService
    {
        ArticleDto GetArticle(int id);
        int CreateArticle(ArticleDto subject);
        void UpdateArticle(ArticleDto subject);
        IList<FieldDefinitionDto> GetProperties();
        ArticleDto[] GetArticles(int organizationId);
        void DeleteArticle(ArticleDto subject);

        IList<AvailabilityDto> GetAvailability(int id);

        void AddImage(int articleId, ImageDto subject);
        void DeleteImage(int articleId, string imageName);
        IList<ImageDto> GetImages(int articleId);

        PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query, int? organization);
    }
}