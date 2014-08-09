namespace Phundus.Core.Shop.Queries.Models
{
    using System;
    using Contracts.Repositories;
    using IdentityAndAccess.Queries;

    public class ContractReadModelReader : ReadModelReaderBase, IContractQueries
    {
        public IContractRepository Repository { get; set; }

        public ContractDetailDto FindContract(int contractId, int organizationId, int currentUserId)
        {
            var result = Repository.ById(contractId);
            return new ContractDetailDto
            {
                Id = result.Id,
                CreatedOn = result.CreatedOn,
                OrganizationId = result.OrganizationId
            };
        }
    }

    public class ContractDetailDto
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int OrganizationId { get; set; }
    }
}