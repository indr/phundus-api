namespace Phundus.Inventory.Projections
{
    using System.Collections.Generic;
    using Articles.Model;
    using AutoMapper;
    using Common.Domain.Model;
    using Common.Querying;
    using Model.Articles;

    public interface IImagesQueries
    {
        IEnumerable<ImageData> ByArticle(ArticleId articleId);
    }

    public class ImageQueries : QueryBase, IImagesQueries
    {
        static ImageQueries()
        {
            Mapper.CreateMap<Image, ImageData>();
        }

        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<ImageData> ByArticle(ArticleId articleId)
        {
            var article = ArticleRepository.GetById(articleId);

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