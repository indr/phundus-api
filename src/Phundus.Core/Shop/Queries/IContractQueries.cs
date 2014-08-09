namespace Phundus.Core.Shop.Queries
{
    public interface IContractQueries
    {
        ContractDto FindContract(int contractId, int organizationId, int currentUserId);
    }
}