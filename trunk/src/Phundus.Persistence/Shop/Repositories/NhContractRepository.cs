namespace Phundus.Persistence.Shop.Repositories
{
    using Phundus.Shop.Contracts.Model;
    using Phundus.Shop.Contracts.Repositories;

    public class NhContractRepository : NhRepositoryBase<Contract>, IContractRepository
    {
        public new int Add(Contract entity)
        {
            base.Add(entity);
            return entity.Id;
        }

        public Contract GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new ContractNotFoundException(id);
            return result;
        }
    }
}