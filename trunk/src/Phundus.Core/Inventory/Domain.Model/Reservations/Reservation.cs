﻿namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Shop.Domain.Model.Ordering;

    public class Reservation : EventSourcedRootEntity
    {
        private int _amount;
        private ArticleId _articleId;
        private OrganizationId _organizationId;
        private ReservationId _reservationId;
        private TimeRange _timeRange;

        public Reservation(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId, OrderId orderId, CorrelationId correlationId,
            TimeRange timeRange, int amount)
        {
            Apply(new ArticleReserved(organizationId, articleId, reservationId, orderId, correlationId, timeRange, amount));
        }

        public Reservation(IEnumerable<IDomainEvent> eventStream, long eventStreamVersion) : base(eventStream, eventStreamVersion)
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

        public TimeRange TimeRange
        {
            get { return _timeRange; }
        }

        public int Amount
        {
            get { return _amount; }
        }

        protected void When(ArticleReserved e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _reservationId = new ReservationId(e.ReservationId);
            _timeRange = new TimeRange(e.FromUtc, e.ToUtc);
            _amount = e.Amount;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return _organizationId;
            yield return _articleId;
            yield return _reservationId;
        }


        public void ChangeTimeRange(TimeRange timeRange)
        {
            if (timeRange == null)
                throw new ArgumentNullException("timeRange", @"Time range can not be null.");
            Apply(new ReservationTimeRangeChanged(_organizationId, _articleId, _reservationId, timeRange));
        }

        protected void When(ReservationTimeRangeChanged e)
        {
            _timeRange = new TimeRange(e.FromUtc, e.ToUtc);
        }

        public void ChangeAmount(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("amount", @"Amount must be greater than zero.");
            Apply(new ReservationAmountChanged(_organizationId, _articleId, _reservationId, amount));
        }

        protected void When(ReservationAmountChanged e)
        {
            _amount = e.Amount;
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }
    }
}