namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;
    using AutoMapper;
    using Cqrs;
    using Model;
    using Repositories;

    public interface IImageQueries
    {
        IEnumerable<ImageDto> ByArticle(int articleId);
    }

    public class ImageReadModel : ReadModelBase, IImageQueries
    {
        static ImageReadModel()
        {
            Mapper.CreateMap<Image, ImageDto>();
        }

        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<ImageDto> ByArticle(int articleId)
        {
            var article = ArticleRepository.ById(articleId);

            return Mapper.Map<IEnumerable<ImageDto>>(article.Images);
        }
    }
}