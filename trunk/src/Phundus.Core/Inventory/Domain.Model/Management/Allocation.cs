namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Reservations;

    public class Allocation : ValueObject
    {
        public Allocation(AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            AllocationId = allocationId;
            ReservationId = reservationId;
            Period = period;
            Quantity = quantity;
        }

        public AllocationId AllocationId { get; private set; }

        public ReservationId ReservationId { get; private set; }

        public Period Period { get; private set; }

        public int Quantity { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AllocationId;
        }

        public void ChangeQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }

        public void ChangePeriod(Period newPeriod)
        {
            Period = newPeriod;
        }
    }
}