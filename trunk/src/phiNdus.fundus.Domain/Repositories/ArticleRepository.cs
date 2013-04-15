using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        private IQueryable<Article> Articles
        {
            get { return Session.Query<Article>(); }
        }

        #region IArticleRepository Members

        public ICollection<Article> FindAll(Organization selectedOrganization)
        {
            var query = from a in Articles where a.Organization.Id == selectedOrganization.Id select a;
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

        #endregion
    }
}