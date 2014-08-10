namespace Phundus.Core.Shop.Queries
{
    using System.Collections.Generic;

    public interface IContractQueries
    {
        ContractDto FindContract(int contractId, int organizationId, int currentUserId);
        IEnumerable<ContractDto> FindContracts(int organizationId, int currentUserId);
    }
}