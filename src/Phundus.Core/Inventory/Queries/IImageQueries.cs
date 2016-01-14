namespace Phundus.Inventory.Queries
{
    using System.Collections.Generic;
    using Articles.Model;
    using Articles.Repositories;
    using AutoMapper;
    using Cqrs;

    public interface IImageQueries
    {
        IEnumerable<ImageDto> ByArticle(int articleId);
    }

    public class ImageReadModel : AutoMappingReadModelBase, IImageQueries
    {
        static ImageReadModel()
        {
            Mapper.CreateMap<Image, ImageDto>();
        }

        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<ImageDto> ByArticle(int articleId)
        {
            var article = ArticleRepository.GetById(articleId);

            return Mapper.Map<IEnumerable<ImageDto>>(article.Images);
        }
    }
}