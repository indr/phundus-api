namespace Phundus.Core.Inventory._Legacy.Services
{
    using System.Collections.Generic;
    using Dtos;
    using Queries;

    public interface IArticleService
    {
        
        void UpdateArticle(ArticleDto subject, int organizationId);
        
        IList<AvailabilityDto> GetAvailability(int id);

        void AddImage(int articleId, ImageDto subject, int organizationId);
        void DeleteImage(int articleId, string imageName, int organizationId);
        IList<ImageDto> GetImages(int articleId);
    }
}