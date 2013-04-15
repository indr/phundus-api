namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using phiNdus.fundus.Domain.Entities;
    using piNuts.phundus.Infrastructure;

    public interface ISettingRepository : IRepository<Setting>
    {
        Setting FindByKey(string key);
        IDictionary<string, Setting> FindByKeyspace(string keyspace);
    }
}