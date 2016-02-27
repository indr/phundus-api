namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using FluentMigrator;

    [Migration(201602270155)]
    public class M201602270155_Upgrade_Order_Events : EventMigrationBase
    {
        private IDictionary<int, Guid> _orderIdMap = new Dictionary<int, Guid>();
        private IDictionary<Guid, DateTime?> _orderModifiedAtUtcMap = new Dictionary<Guid, DateTime?>();
        private IDictionary<Guid, int> _orderStatusMap = new Dictionary<Guid, int>();

        protected override void Migrate()
        {
            FillMaps();

            UpgradeOrderApproved();
            UpgradeOrderRejected();
            UpgradeOrderClosed();
            UpgradeOrderPlaced();
        }

        private void UpgradeOrderPlaced()
        {
            var storedEvents = FindStoredEvents("Phundus.Shop.Orders.Model.OrderPlaced, Phundus.Core");
            var domainEvents = storedEvents.Select(Deserialize<OrderPlaced>);

            foreach (var each in domainEvents)
            {
                var aggregateId = _orderIdMap[each.OrderShortId];
                UpdateStoredEventAggregateId(each.EventGuid, aggregateId);
            }
            ;
        }

        private void UpgradeOrderClosed()
        {
            var storedEvents = FindStoredEvents("Phundus.Shop.Orders.Model.OrderClosed, Phundus.Core");
            var domainEvents = storedEvents.Select(Deserialize<OrderClosed>);

            foreach (var each in domainEvents)
            {
                if (each.OrderId != Guid.Empty)
                    continue;

                each.OrderId = _orderIdMap[each.OrderShortId];
                UpdateStoredEvent(each.EventGuid, each, each.OrderId);
            }
            ;
        }

        private void UpgradeOrderRejected()
        {
            var storedEvents = FindStoredEvents("Phundus.Shop.Orders.Model.OrderRejected, Phundus.Core");
            var domainEvents = storedEvents.Select(Deserialize<OrderRejected>);

            foreach (var each in domainEvents)
            {
                if (each.OrderId != Guid.Empty)
                    continue;

                each.OrderId = _orderIdMap[each.OrderShortId];
                UpdateStoredEvent(each.EventGuid, each, each.OrderId);
            }
            ;
        }

        private void UpgradeOrderApproved()
        {
            var storedEvents = FindStoredEvents("Phundus.Shop.Orders.Model.OrderApproved, Phundus.Core");
            var domainEvents = storedEvents.Select(Deserialize<OrderApproved>);

            foreach (var each in domainEvents)
            {
                if (each.OrderId != Guid.Empty)
                    continue;

                each.OrderId = _orderIdMap[each.OrderShortId];
                UpdateStoredEvent(each.EventGuid, each, each.OrderId);
            }
        }

        private void FillMaps()
        {
            var command =
                CreateCommand(
                    "SELECT [OrderGuid] AS OrderId, [Id] AS OrderShortId, [Status], [ModifiedUtc] AS ModifiedAtUtc FROM [Dm_Shop_Order]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var orderId = reader.GetGuid(0);
                    var orderShortId = reader.GetInt32(1);
                    var status = reader.GetInt32(2);
                    var modifiedAtUtc = reader.IsDBNull(3) ? null : (DateTime?) reader.GetDateTime(3);

                    _orderIdMap.Add(orderShortId, orderId);
                    _orderModifiedAtUtcMap.Add(orderId, modifiedAtUtc);
                    _orderStatusMap.Add(orderId, status);
                }
            }
        }

        [DataContract]
        public class OrderApproved : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; set; }
        }

        [DataContract]
        public class OrderClosed : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; set; }
        }

        [DataContract]
        public class OrderPlaced : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; set; }
        }

        [DataContract]
        public class OrderRejected : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; set; }
        }
    }
}