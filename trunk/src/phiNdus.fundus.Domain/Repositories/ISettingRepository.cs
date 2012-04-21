using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface ISettingRepository : IRepository<Setting>
    {
        Setting FindByKey(string key);
        IDictionary<string, Setting> FindByKeyspace(string keyspace);
    }
}