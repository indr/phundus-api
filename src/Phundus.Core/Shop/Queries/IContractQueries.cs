namespace Phundus.Core.Shop.Queries
{
    using Models;

    public interface IContractQueries
    {
        ContractDetailDto FindContract(int contractId, int organizationId, int currentUserId);
    }
}