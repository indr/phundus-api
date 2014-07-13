namespace Phundus.Core.Repositories
{
    using System.Collections.Generic;
    using Phundus.Core.Entities;

    public interface ISettingRepository : IRepository<Setting>
    {
        Setting FindByKey(string key);
        IDictionary<string, Setting> FindByKeyspace(string keyspace);
    }
}