namespace Phundus.Core.Shop.Domain.Model.Renting
{
    using System;
    using Infrastructure;

    public interface IContractRepository : IRepository<Contract>
    {
        new int Add(Contract entity);
        Contract GetById(int id);
    }

    public class ContractNotFoundException : Exception
    {
        public ContractNotFoundException(int id) : base(String.Format("Der Vertag mit der Id {0} konnte nicht gefunden werden.", id))
        {
            
        }
    }
}