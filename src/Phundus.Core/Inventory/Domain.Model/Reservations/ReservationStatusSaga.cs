namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using Common.Domain.Model;

    public class ReservationStatusSaga :
        StateMachineSagaBase<ReservationStatusSaga.State, ReservationStatusSaga.Trigger>
    {
        public enum Trigger
        {
        }

        public enum State
        {
            Dummy
        }

        public ReservationStatusSaga() : base(State.Dummy)
        {
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic)e);
        }

        private void When(DomainEvent e)
        {
            // Fallback
        }
    }
}