namespace Phundus.Core.Shop.Contracts.Repositories
{
    using Infrastructure;
    using Model;

    public interface IContractRepository : IRepository<Contract>
    {
        new int Add(Contract entity);
    }
}