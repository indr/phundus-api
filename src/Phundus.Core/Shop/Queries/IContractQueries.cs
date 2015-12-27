namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IContractQueries
    {
        ContractDto FindContract(int contractId, Guid organizationId, int currentUserId);
        IEnumerable<ContractDto> FindContracts(Guid organizationId, int currentUserId);
    }
}