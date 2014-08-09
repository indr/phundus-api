namespace Phundus.Rest.Controllers.Shop
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Contracts.Commands;
    using Core.Shop.Queries;
    using Exceptions;

    public class ContractsController : ApiControllerBase
    {
        public IContractQueries ContractQueries { get; set; }

        [Transaction]
        public virtual ContractDetailDoc Get(int organizationId, int id)
        {
            var result = ContractQueries.FindContract(id, organizationId, CurrentUserId);
            if (result == 0)
                throw new HttpNotFoundException();

            return new ContractDetailDoc
            {
                Id = result,
                CreatedOn = DateTime.Now
            };
        }

        public class ContractsPostDoc
        {
            public int UserId { get; set; }
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

            return Request.CreateResponse(HttpStatusCode.Created, command.ContractId);
        }
    }

    public class ContractDetailDoc
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}