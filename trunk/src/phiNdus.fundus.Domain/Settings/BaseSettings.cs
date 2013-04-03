namespace phiNdus.fundus.Domain.Settings
{
    using System.Collections.Generic;
    using Entities;
    using Repositories;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    public class BaseSettings
    {
        private readonly string _keyspace;
        private IDictionary<string, Setting> _values;

        public BaseSettings(string keyspace)
        {
            _keyspace = keyspace;
            _values = null;
        }

        private IDictionary<string, Setting> Values
        {
            get { return _values ?? (_values = Load()); }
        }

        protected string Keyspace
        {
            get { return _keyspace; }
        }

        private IDictionary<string, Setting> Load()
        {
            using (UnitOfWork.Start())
            {
                var repo = GlobalContainer.Resolve<ISettingRepository>();
                return repo.FindByKeyspace(_keyspace);
            }
        }

        protected string GetString(string key)
        {
            return GetString(key, null);
        }

        protected string GetString(string key, string def)
        {
            Setting setting;
            return Values.TryGetValue(key, out setting) ? setting.StringValue : def;
        }
    }
}