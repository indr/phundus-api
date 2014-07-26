﻿namespace Phundus.Persistence.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.InventoryCtx.Model;
    using Core.InventoryCtx.Repositories;
    using NHibernate.Criterion;
    using NHibernate.Linq;

    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        private IQueryable<Article> Articles
        {
            get { return Session.Query<Article>(); }
        }

        public ICollection<Article> FindAll(int organizationId)
        {
            var query = from a in Articles where a.Organization.Id == organizationId select a;
            return query.ToList();
        }

        public ICollection<Article> FindMany(string query, int? organization, int start, int count, out int total)
        {
            var q = Session.QueryOver<Article>();

            if (organization.HasValue)
                q = q.Where(a => a.Organization.Id == organization.Value);

            var d = q.JoinQueryOver<FieldValue>(a => a.FieldValues)
                .WhereRestrictionOn(fv => fv.TextValue).IsLike(query, MatchMode.Anywhere)
                .List().Distinct();
            total = d.Count();
            return d.Skip(start).Take(count).ToList();
        }
    }
}