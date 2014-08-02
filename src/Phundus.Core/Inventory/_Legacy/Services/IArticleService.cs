namespace Phundus.Core.Inventory._Legacy.Services
{
    using System;
    using System.Collections.Generic;
    using Cqrs.Paging;
    using Dtos;

    public interface IArticleService
    {
        int CreateArticle(ArticleDto subject, int organizationId);
        void UpdateArticle(ArticleDto subject, int organizationId);
        IList<FieldDefinitionDto> GetProperties();
        
        void DeleteArticle(ArticleDto subject, int organizationId);

        IList<AvailabilityDto> GetAvailability(int id);

        void AddImage(int articleId, ImageDto subject, int organizationId);
        void DeleteImage(int articleId, string imageName, int organizationId);
        IList<ImageDto> GetImages(int articleId);
    }
}