namespace Phundus.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public interface IArticleQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        ArticleDto GetById(int id);

        IEnumerable<ArticleDto> FindByOwnerId(Guid ownerId);

        IEnumerable<ArticleDto> Query(InitiatorId initiatorId, OwnerId queryOwnerId, string query);
        ArticleDto GetById(Guid articleGuid);
    }
}