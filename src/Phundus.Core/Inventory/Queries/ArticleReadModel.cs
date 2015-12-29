namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Articles.Repositories;
    using Cqrs;

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleRepository ArticleRepository { get; set; }

        public ArticleDto GetById(int id)
        {
            var article = ArticleRepository.FindById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler().CreateDto(article);
        }

        public IEnumerable<ArticleDto> FindByOwnerId(Guid ownerId)
        {
            var articles = ArticleRepository.FindByOwnerId(ownerId);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }
    }
}