namespace Phundus.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Articles.Repositories;
    using Common;
    using Common.Domain.Model;
    using Cqrs;

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleRepository ArticleRepository { get; set; }

        public ArticleDto GetById(int id)
        {
            var article = ArticleRepository.FindById(id);
            if (article == null)
                throw new NotFoundException(String.Format("Article {0} not found.", id));
            return new ArticleDtoAssembler().CreateDto(article);
        }

        public IEnumerable<ArticleDto> FindByOwnerId(Guid ownerId)
        {
            var articles = ArticleRepository.FindByOwnerId(ownerId);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }

        public IEnumerable<ArticleDto> Query(InitiatorId initiatorId, OwnerId queryOwnerId, string query)
        {
            var articles = ArticleRepository.Query(initiatorId, queryOwnerId, query);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }
    }
}