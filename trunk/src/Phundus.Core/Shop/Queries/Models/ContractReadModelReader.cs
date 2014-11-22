namespace Phundus.Core.Shop.Queries.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Ddd;
    using Domain.Model.Renting;

    public class ContractReadModelReader : ReadModelReaderBase, IContractQueries
    {
        public ContractDto FindContract(int contractId, int organizationId, int currentUserId)
        {
            // TODO: Read-Model-Security
            return (from c in CreateCtx().ContractDtos
                where c.Id == contractId && c.OrganizationId == organizationId
                select c)
                .FirstOrDefault();
        }

        public IEnumerable<ContractDto> FindContracts(int organizationId, int currentUserId)
        {
            // TODO: Read-Model-Security
            return (from c in CreateCtx().ContractDtos
                where c.OrganizationId == organizationId
                select c);
        }
    }

    public class ConractReadModelWriter : ReadModelWriterBase, ISubscribeTo<ContractCreated>
    {
        public void Handle(ContractCreated @event)
        {
            var entity = new ContractDto
            {
                Id = @event.ContractId,
                OrganizationId = @event.OrganizationId,
                CreatedOn = @event.OccuredOnUtc,
                BorrowerId = @event.BorrowerId,
                BorrowerFirstName = @event.BorrowerFirstName,
                BorrowerLastName = @event.BorrowerLastName,
                BorrowerEmail = @event.BorrowerEmail
            };
            Ctx.ContractDtos.InsertOnSubmit(entity);
            Ctx.SubmitChanges();
        }
    }
}