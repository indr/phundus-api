using System;
using System.Collections.Generic;
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

        public IDictionary<string, Setting> FindByKeyspace(string keyspace)
        {
            Guard.Against<ArgumentException>(keyspace.EndsWith("."), "Keyspace darf nicht mit einem Punkt enden");

            var query = from s in Settings
                        where s.Key.StartsWith(keyspace)
                        select s;
            return query.ToDictionary(s => s.Key.Remove(0, keyspace.Length + 1), s => s);
        }

        private IQueryable<Setting> Settings
        {
            get { return Session.Query<Setting>(); }
        }
    }
}