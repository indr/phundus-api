namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Contracts.Commands;
    using Core.Shop.Queries;

    [RoutePrefix("api/organizations/{organizationId}/contracts")]
    public class OrganizationsContractsController : ApiControllerBase
    {
        public IContractQueries ContractQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId)
        {
            var result = ContractQueries.FindContracts(organizationId, CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, ToDocs(result));
        }

        [GET("{contractId}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int contractId)
        {
            var result = ContractQueries.FindContract(contractId, organizationId, CurrentUserId);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contract not found");

            return Request.CreateResponse(HttpStatusCode.OK, ToDoc(result));
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, ContractsPostDoc doc)
        {
            var command = new CreateEmptyContract
            {
                InitiatorId = CurrentUserId,
                OrganizationId = organizationId,
                UserId = doc.UserId
            };
            Dispatcher.Dispatch(command);

            return Get(organizationId, command.ContractId);
        }

        private static IEnumerable<ContractDoc> ToDocs(IEnumerable<ContractDto> dtos)
        {
            return dtos.Select(each => new ContractDoc
            {
                BorrowerFirstName = each.BorrowerFirstName,
                BorrowerLastName = each.BorrowerLastName,
                ContractId = each.Id,
                CreatedOn = each.CreatedOn,
                OrganizationId = each.OrganizationId
            }).ToList();
        }

        private static ContractDetailDoc ToDoc(ContractDto dto)
        {
            return new ContractDetailDoc
            {
                ContractId = dto.Id,
                CreatedOn = dto.CreatedOn,
                OrganizationId = dto.OrganizationId,
                BorrowerId = dto.BorrowerId,
                BorrowerLastName = dto.BorrowerLastName,
                BorrowerEmail = dto.BorrowerEmail,
                BorrowerFirstName = dto.BorrowerFirstName
            };
        }
    }

    public class ContractsPostDoc
    {
        public int UserId { get; set; }
    }

    public class ContractDoc
    {
        public int ContractId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? SignedOn { get; set; }

        public string BorrowerFirstName { get; set; }
        public string BorrowerLastName { get; set; }
    }

    public class ContractDetailDoc : ContractDoc
    {
        public int BorrowerId { get; set; }
        public string BorrowerEmail { get; set; }
    }
}