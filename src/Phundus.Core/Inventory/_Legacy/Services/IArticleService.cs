namespace Phundus.Core.Inventory._Legacy.Services
{
    using System.Collections.Generic;
    using Queries;

    public interface IArticleService
    {
        IList<AvailabilityDto> GetAvailability(int id);

        IList<ImageDto> GetImages(int articleId);
    }
}