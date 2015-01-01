namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using Common.Domain.Model;

    public interface IReservationSagaRepository
    {
        void Save(ReservationSaga saga);
        ReservationSaga FindByCorrelationId(Guid orderItemId);
    }

    public interface ISagaRepository
    {
        TSaga FindById<TSaga>(string id);
        void Save(ISaga saga);
    }

    public class SagaRepository : ISagaRepository
    {
        public TSaga FindById<TSaga>(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(ISaga saga)
        {
            throw new NotImplementedException();
        }
    }


    public interface ISagaStore
    {
        
    }

    public class SagaStore : ISagaStore

    {
        
    }
}