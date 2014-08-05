namespace Phundus.Core.Inventory._Legacy.Services
{
    using System.Collections.Generic;
    using Dtos;

    public interface IArticleService
    {
        IList<AvailabilityDto> GetAvailability(int id);

        void AddImage(int articleId, ImageDto subject, int organizationId);
        void DeleteImage(int articleId, string imageName, int organizationId);
        IList<ImageDto> GetImages(int articleId);
    }
}