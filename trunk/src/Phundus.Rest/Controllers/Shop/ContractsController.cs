namespace Phundus.Rest.Controllers.Shop
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Castle.Transactions;
    using Core.Shop.Contracts.Commands;
    using Core.Shop.Queries;
    using Core.Shop.Queries.Models;

    public class ContractsController : ApiControllerBase
    {
        public IContractQueries ContractQueries { get; set; }

        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int id)
        {
            var result = ContractQueries.FindContract(id, organizationId, CurrentUserId);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contract not found");

            return Request.CreateResponse(HttpStatusCode.OK, ToDoc(result));
        }

        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, ContractsPostDoc doc)
        {
            var command = new CreateNewEmptyContract
            {
                InitiatorId = CurrentUserId,
                OrganizationId = organizationId,
                UserId = doc.UserId
            };
            Dispatcher.Dispatch(command);

            return Get(organizationId, command.ContractId);
        }

        private static ContractDetailDoc ToDoc(ContractDetailDto result)
        {
            return new ContractDetailDoc
            {
                Id = result.Id,
                CreatedOn = result.CreatedOn,
                OrganizationId = result.OrganizationId
            };
        }
    }

    public class ContractsPostDoc
    {
        public int UserId { get; set; }
    }

    public class ContractDetailDoc
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int OrganizationId { get; set; }
    }
}