namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;

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
    }
}