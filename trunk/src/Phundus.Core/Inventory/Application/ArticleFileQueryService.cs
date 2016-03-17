namespace Phundus.Inventory.Projections
{
    using System.Collections.Generic;
    using AutoMapper;
    using Common.Domain.Model;
    using Common.Querying;
    using Model.Articles;

    public interface IArticleFileQueryService
    {
        IEnumerable<ImageData> ByArticle(ArticleId articleId);
    }

    public class ArticleFileQueryService : QueryServiceBase, IArticleFileQueryService
    {
        static ArticleFileQueryService()
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
        public string FileName { get; set; }
        public string Type { get; set; }
        public bool IsPreview { get; set; }
        public long Length { get; set; }
    }
}