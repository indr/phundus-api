namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;
    using Common.Extensions;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Shop.Domain.Model.Ordering;

    public class Reservation : EventSourcedRootEntity
    {
        private ArticleId _articleId;
        private OrganizationId _organizationId;
        private Period _period;
        private int _quantity;
        private ReservationId _reservationId;
        private ReservationStatus _status;

        public Reservation(ReservationId reservationId, OrganizationId organizationId, ArticleId articleId,
            OrderId orderId, Period period, int amount)
        {
            Apply(new ArticleReserved(organizationId, articleId, reservationId, orderId, period, amount, ReservationStatus.New));
        }

        public Reservation(IEnumerable<IDomainEvent> eventStream, long eventStreamVersion)
            : base(eventStream, eventStreamVersion)
        {
        }

        public ArticleId ArticleId
        {
            get { return _articleId; }
        }

        public OrganizationId OrganizationId
        {
            get { return _organizationId; }
        }

        public ReservationId ReservationId
        {
            get { return _reservationId; }
        }

        public Period Period
        {
            get { return _period; }
        }

        public int Quantity
        {
            get { return _quantity; }
        }

        public ReservationStatus Status
        {
            get { return _status; }
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        protected void When(ArticleReserved e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _reservationId = new ReservationId(e.ReservationId);
            _period = new Period(e.FromUtc, e.ToUtc);
            _quantity = e.Quantity;
            _status = EnumHelper.Parse<ReservationStatus>(e.ReservationStatus);
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return _organizationId;
            yield return _articleId;
            yield return _reservationId;
        }

        public void ChangePeriod(Period period)
        {
            EnsureNotCancelled();

            if (period == null)
                throw new ArgumentNullException("period", @"Period can not be null.");
            Apply(new ReservationPeriodChanged(_organizationId, _articleId, _reservationId, Period, period));
        }

        protected void When(ReservationPeriodChanged e)
        {
            _period = e.NewPeriod;
        }

        public void ChangeQuantity(int quantity)
        {
            EnsureNotCancelled();

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("quantity", @"Quantity must be greater than zero.");
            Apply(new ReservationQuantityChanged(_organizationId, _articleId, _reservationId, Quantity, quantity));
        }

        protected void When(ReservationQuantityChanged e)
        {
            _quantity = e.NewQuantity;
        }

        public virtual void Cancel()
        {
            Apply(new ReservationCancelled(_organizationId, _articleId, _reservationId, _period, _quantity));
        }

        protected void When(ReservationCancelled e)
        {
            _status = ReservationStatus.Cancelled;
        }

        private void EnsureNotCancelled()
        {
            if (Status == ReservationStatus.Cancelled)
                throw new InvalidOperationException("A cancelled reservation can not be modified.");
        }
    }
}