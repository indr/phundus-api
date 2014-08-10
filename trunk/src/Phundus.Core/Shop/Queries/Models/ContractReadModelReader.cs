namespace Phundus.Core.Shop.Queries.Models
{
    using System.Linq;
    using Contracts.Model;
    using Ddd;

    public class ContractReadModelReader : ReadModelReaderBase, IContractQueries
    {
        public ContractDto FindContract(int contractId, int organizationId, int currentUserId)
        {
            return (from c in Ctx.ContractDtos
                where c.Id == contractId && c.OrganizationId == organizationId
                select c)
                .FirstOrDefault();
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
                CreatedOn = @event.CreatedOn,
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