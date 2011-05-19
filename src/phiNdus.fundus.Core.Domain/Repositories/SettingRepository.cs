using System;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class SettingRepository : NHRepository<Setting>, ISettingRepository
    {
        public Setting FindByKey(string key)
        {
            var query = from s in Settings
                        where s.Key == key 
                        select s;
            return query.FirstOrDefault();
        }

        private IQueryable<Setting> Settings
        {
            get { return Session.Query<Setting>(); }
        }
    }
}