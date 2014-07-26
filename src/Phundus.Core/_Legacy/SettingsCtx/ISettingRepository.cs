namespace Phundus.Core.SettingsCtx
{
    using System.Collections.Generic;
    using Infrastructure;

    public interface ISettingRepository : IRepository<Setting>
    {
        Setting FindByKey(string key);
        IDictionary<string, Setting> FindByKeyspace(string keyspace);
    }
}