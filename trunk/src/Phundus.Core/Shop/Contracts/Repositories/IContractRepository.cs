namespace Phundus.Core.Shop.Contracts.Repositories
{
    using Infrastructure;
    using Model;

    public interface IContractRepository : IRepository<Contract>
    {
        new int Add(Contract entity);
        Contract ById(object id);
        Contract GetById(object id);
    }
}