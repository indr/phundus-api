﻿namespace Phundus.Persistence.Shop.Repositories
{
    using Core.Shop.Contracts.Model;
    using Core.Shop.Contracts.Repositories;

    public class NhContractRepository : NhRepositoryBase<Contract>, IContractRepository
    {
        public new int Add(Contract entity)
        {
            base.Add(entity);
            return entity.Id;
        }
    }
}