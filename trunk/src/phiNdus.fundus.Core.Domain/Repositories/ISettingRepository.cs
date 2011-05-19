﻿using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public interface ISettingRepository : IRepository<Setting>
    {
        Setting FindByKey(string key);
    }
}