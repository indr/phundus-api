﻿namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentMigrator;

    [Migration(201602270318)]
    public class M201602270318_Generate_Missing_OrderStatus_Events : EventMigrationBase
    {
        private IDictionary<Guid, int> _orderIdMap = new Dictionary<Guid, int>();
        private IDictionary<Guid, DateTime?> _orderModifiedAtUtcMap = new Dictionary<Guid, DateTime?>();
        private IDictionary<Guid, int> _orderStatusMap = new Dictionary<Guid, int>();
        private IList<StoredEvent> _approvedEvents;
        private IList<StoredEvent> _rejectedEvents;
        private IList<StoredEvent> _closedEvents;
        private string _approvedTypeName;
        private string _rejectedTypeName;
        private string _closedTypeName;

        protected override void Migrate()
        {
            FillMaps();

            GenerateEvents();
        }

        private void GenerateEvents()
        {
            _approvedTypeName = "Phundus.Shop.Orders.Model.OrderApproved, Phundus.Core";
            _approvedEvents = FindStoredEvents(_approvedTypeName);
            _rejectedTypeName = "Phundus.Shop.Orders.Model.OrderRejected, Phundus.Core";
            _rejectedEvents = FindStoredEvents(_rejectedTypeName);
            _closedTypeName = "Phundus.Shop.Orders.Model.OrderClosed, Phundus.Core";
            _closedEvents = FindStoredEvents(_closedTypeName);
            
            foreach (var each in _orderStatusMap)
            {
                if (each.Value == 0)
                    continue;

                if (each.Value == 2)
                    FindOrGenerateApproved(each.Key);
                else if (each.Value == 3)
                    FindOrGenerateRejected(each.Key);
                else if (each.Value == 4)
                    FindOrGenerateClosed(each.Key);
            }
        }

        private void FindOrGenerateClosed(Guid orderId)
        {
            var e = _closedEvents.SingleOrDefault(p => p.AggregateId == orderId);
            if (e != null)
                return;
            
            InsertStoredEvent(_orderModifiedAtUtcMap[orderId].Value, _closedTypeName, new M201602270155_Upgrade_Order_Events.OrderClosed
            {
                OrderId = orderId,
                OrderShortId = _orderIdMap[orderId]
            });
        }

        private void FindOrGenerateRejected(Guid orderId)
        {
            var e = _rejectedEvents.SingleOrDefault(p => p.AggregateId == orderId);
            if (e != null)
                return;

            InsertStoredEvent(_orderModifiedAtUtcMap[orderId].Value, _rejectedTypeName, new M201602270155_Upgrade_Order_Events.OrderRejected
            {
                OrderId = orderId,
                OrderShortId = _orderIdMap[orderId]
            });
        }

        private void FindOrGenerateApproved(Guid orderId)
        {
            var e = _approvedEvents.SingleOrDefault(p => p.AggregateId == orderId);
            if (e != null)
                return;

            InsertStoredEvent(_orderModifiedAtUtcMap[orderId].Value, _approvedTypeName, new M201602270155_Upgrade_Order_Events.OrderApproved
            {
                OrderId = orderId,
                OrderShortId = _orderIdMap[orderId]
            });
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
                    var modifiedAtUtc = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);

                    _orderIdMap.Add(orderId, orderShortId);
                    _orderModifiedAtUtcMap.Add(orderId, modifiedAtUtc);
                    _orderStatusMap.Add(orderId, status);
                }
            }
        }
    }
}