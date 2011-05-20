using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public class BaseSettings
    {
        private readonly string _keyspace;
        private IDictionary<string, Setting> _values;

        public BaseSettings(string keyspace)
        {
            _keyspace = keyspace;
            _values = null;
        }

        private IDictionary<string, Setting> Load()
        {
            using (UnitOfWork.Start())
            {
                var repo = IoC.Resolve<ISettingRepository>();
                return repo.FindByKeyspace(_keyspace);
            }
        }

        private IDictionary<string, Setting> Values
        {
            get { return _values ?? (_values = Load()); }
        }

        protected string Keyspace { get { return _keyspace; } }

        protected string GetString(string key)
        {
            Setting setting;
            return Values.TryGetValue(key, out setting) ? setting.String : null;
        }
    }
}