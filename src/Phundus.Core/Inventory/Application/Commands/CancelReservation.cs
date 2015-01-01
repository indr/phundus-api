namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;

    public class CancelReservation : ICommand
    {
        public CancelReservation(CorrelationId correlationId)
        {
            AssertionConcern.AssertArgumentNotNull(correlationId, "Correlation id must be provided.");

            CorrelationId = correlationId;
        }

        public CorrelationId CorrelationId { get; private set; }
    }

    public class ChangeReservationQuantity : ICommand
    {
        public ChangeReservationQuantity(CorrelationId correlationId)
        {
            AssertionConcern.AssertArgumentNotNull(correlationId, "Correlation id must be provided.");

            CorrelationId = correlationId;
        }

        public CorrelationId CorrelationId { get; private set; }
    }

    public class ChangeReservationPeriod : ICommand
    {
        public ChangeReservationPeriod(CorrelationId correlationId)
        {
            AssertionConcern.AssertArgumentNotNull(correlationId, "Correlation id must be provided.");

            CorrelationId = correlationId;
        }

        public CorrelationId CorrelationId { get; private set; }
    }
}