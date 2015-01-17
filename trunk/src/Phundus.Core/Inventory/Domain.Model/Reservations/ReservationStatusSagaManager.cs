namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;

    public class ReservationStatusSagaManager : SagaManager<ReservationStatusSaga>
    {
        public ReservationStatusSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher) : base(repository, dispatcher)
        {
        }
    }
}