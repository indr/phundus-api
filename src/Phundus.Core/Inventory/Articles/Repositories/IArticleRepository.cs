﻿namespace Phundus.Core.Inventory.Articles.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IArticleRepository : IRepository<Article>
    {
        new int Add(Article entity);
        IEnumerable<Article> FindByOwnerId(Guid ownerId);
        Article GetById(int articleId);
        Article GetById(Guid ownerId, int articleId);
        IEnumerable<Article> Query(CurrentUserId currentUserId, OwnerId queryOwnerId, string query);
    }
}