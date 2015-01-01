namespace Phundus.Persistence
{
    using System;
    using Common.Domain.Model;

    public class SagaRepository : ISagaRepository
    {
        public TSaga FindById<TSaga>(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(ISaga saga)
        {
            throw new NotImplementedException();
        }
    }
}