namespace Phundus.Rest.Controllers.Shop
{
    using System;
    using System.Web.Http;
    using Core.Shop.Contracts.Commands;
    using Core.Shop.Queries;
    using Exceptions;

    public class ContractsController : ApiControllerBase
    {
        public IContractQueries ContractQueries { get; set; }

        public ContractDetailDoc Get(int organizationId, int contractId)
        {
            var result = ContractQueries.FindContract(contractId, organizationId, CurrentUserId);
            if (result == 0)
                throw new HttpNotFoundException();

            return new ContractDetailDoc
            {
                Id = result,
                CreatedOn = DateTime.Now
            };
        }

        public int Post(int organizationId, [FromBody] int userId)
        {
            var command = new CreateNewEmptyContract
            {
                InitiatorId = CurrentUserId,
                OrganizationId = organizationId,
                UserId = userId
            };
            Dispatcher.Dispatch(command);

            return command.ContractId;
        }
    }

    public class ContractDetailDoc
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}