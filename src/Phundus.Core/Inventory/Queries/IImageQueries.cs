namespace Phundus.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Articles.Model;
    using Articles.Repositories;
    using AutoMapper;
    using Cqrs;
    using NHibernate;

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

        public ImageReadModel(Func<ISession> sessionFactory) : base(sessionFactory)
        {
        }

        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<ImageDto> ByArticle(int articleId)
        {
            var article = ArticleRepository.GetById(articleId);

            return Mapper.Map<IEnumerable<ImageDto>>(article.Images);
        }
    }
}