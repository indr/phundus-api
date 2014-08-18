namespace Phundus.Core.Shop.Contracts.Repositories
{
    using System;
    using Infrastructure;
    using Model;

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