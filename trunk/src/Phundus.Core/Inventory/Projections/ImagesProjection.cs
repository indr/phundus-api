namespace Phundus.Inventory.Projections
{
    using System.Collections.Generic;
    using Articles.Model;
    using Articles.Repositories;
    using AutoMapper;
    using Common.Domain.Model;
    using Cqrs;

    public interface IImagesQueries
    {
        IEnumerable<ImageData> ByArticle(int articleId);
    }

    public class ImagesReadModel : AutoMappingReadModelBase, IImagesQueries
    {
        static ImagesReadModel()
        {
            Mapper.CreateMap<Image, ImageData>();
        }

        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<ImageData> ByArticle(int articleId)
        {
            var article = ArticleRepository.GetById(new ArticleShortId(articleId));

            return Mapper.Map<IEnumerable<ImageData>>(article.Images);
        }
    }

    public class ImageData
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public bool IsPreview { get; set; }
        public long Length { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
    }
}