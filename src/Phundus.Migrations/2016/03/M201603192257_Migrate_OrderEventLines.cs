namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201603192257)]
    public class M201603192257_Migrate_OrderEventLines : EventMigrationBase
    {
        private readonly Guid _storeId = new Guid("8CFCC30B-5577-441A-A176-FFB449856F71"); // piNuts default store
        private Dictionary<Guid, Guid> _articleIdToStoreIdMap;

        protected override void Migrate()
        {
            CreateArticleIdToStoreIdMap();

            MigrateEvents<OrderCreated>(@"Phundus.Shop.Orders.Model.OrderCreated", e => e.Lines);
            MigrateEvents<OrderItemAdded>(@"Phundus.Shop.Orders.Model.OrderItemAdded", e => e.OrderLine);
            MigrateEvents<OrderItemPeriodChanged>(@"Phundus.Shop.Orders.Model.OrderItemPeriodChanged", e => e.OrderLine);
            MigrateEvents<OrderItemQuantityChanged>(@"Phundus.Shop.Orders.Model.OrderItemQuantityChanged",e => e.OrderLine);
            MigrateEvents<OrderItemRemoved>(@"Phundus.Shop.Orders.Model.OrderItemRemoved", e => e.OrderLine);
            MigrateEvents<OrderItemTotalChanged>(@"Phundus.Shop.Orders.Model.OrderItemTotalChanged", e => e.OrderLine);
            MigrateEvents<OrderPlaced>(@"Phundus.Shop.Orders.Model.OrderPlaced", e => e.Items);
            MigrateEvents<OrderApproved>(@"Phundus.Shop.Orders.Model.OrderApproved", e => e.Items);
            MigrateEvents<OrderRejected>(@"Phundus.Shop.Orders.Model.OrderRejected", e => e.Items);
            MigrateEvents<OrderClosed>(@"Phundus.Shop.Orders.Model.OrderClosed", e => e.Items);
        }

        private void MigrateEvents<T>(string typeName, Func<T, OrderEventLine> getLine) where T : MigratingDomainEvent
        {
            MigrateEvents<T>(typeName, e => new[] { getLine(e) });
        }

        private void MigrateEvents<T>(string typeName, Func<T, IList<OrderEventLine>> getLines)
            where T : MigratingDomainEvent
        {
            var events = FindStoredEvents<T>(typeName + ", Phundus.Core");
            foreach (var each in events)
            {
                if (!MigrateLines(getLines(each)))
                    continue;

                UpdateStoredEvent(each.EventGuid, each);
            }
        }

        private bool MigrateLines(IEnumerable<OrderEventLine> lines)
        {
            if (lines == null)
                return false;
            var updated = false;
            foreach (var each in lines)
            {
                if (each.StoreId == Guid.Empty)
                {
                    updated = true;
                    Guid storeId;
                    if (!_articleIdToStoreIdMap.TryGetValue(each.ArticleId, out storeId))
                        throw new Exception("ArticleId " + each.ArticleId + " not in articleId to storeId map.");

                    each.StoreId = storeId;
                }
            }
            return updated;
        }

        private void CreateArticleIdToStoreIdMap()
        {
            _articleIdToStoreIdMap = new Dictionary<Guid, Guid>();
            var command = CreateCommand("SELECT [ArticleId], [StoreId] FROM [Dm_Inventory_Article]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    _articleIdToStoreIdMap.Add(reader.GetGuid(0), reader.GetGuid(1));
                }
            }

            foreach (var each in DeletedArticleIdMap.Values)
            {
                _articleIdToStoreIdMap.Add(each, _storeId);
            }
        }

        [DataContract]
        public class Actor
        {
            [DataMember(Order = 1)]
            public Guid ActorGuid { get; set; }

            [DataMember(Order = 2)]
            public string EmailAddress { get; set; }

            [DataMember(Order = 3)]
            public string FullName { get; set; }
        }

        [DataContract]
        public class Lessee
        {
            [DataMember(Order = 1)]
            public virtual Guid LesseeGuid { get; set; }

            [DataMember(Order = 2)]
            public virtual string FirstName { get; protected set; }

            [DataMember(Order = 3)]
            public virtual string LastName { get; protected set; }

            [DataMember(Order = 4)]
            public virtual string Street { get; protected set; }

            [DataMember(Order = 5)]
            public virtual string Postcode { get; protected set; }

            [DataMember(Order = 6)]
            public virtual string City { get; protected set; }

            [DataMember(Order = 7)]
            public virtual string EmailAddress { get; protected set; }

            [DataMember(Order = 8)]
            public virtual string PhoneNumber { get; protected set; }

            [DataMember(Order = 9)]
            public virtual string MemberNumber { get; protected set; }
        }

        [DataContract]
        public class Lessor
        {
            [DataMember(Order = 1)]
            public virtual Guid LessorGuid { get; set; }

            [DataMember(Order = 2)]
            public virtual string Name { get; set; }

            [DataMember(Order = 3)]
            public virtual bool DoesPublicRental { get; set; }

            [DataMember(Order = 4)]
            public string PostalAddress { get; set; }

            [DataMember(Order = 5)]
            public string PhoneNumber { get; set; }

            [DataMember(Order = 6)]
            public string EmailAddress { get; set; }

            [DataMember(Order = 7)]
            public string Website { get; set; }
        }

        [DataContract]
        public class OrderApproved : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; protected set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 4)]
            public Actor Initiator { get; protected set; }

            [DataMember(Order = 5)]
            public Lessor Lessor { get; protected set; }

            [DataMember(Order = 6)]
            public Lessee Lessee { get; protected set; }

            [DataMember(Order = 7)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 8)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 9)]
            public IList<OrderEventLine> Items { get; protected set; }
        }

        [DataContract]
        public class OrderClosed : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; protected set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 4)]
            public Actor Initiator { get; protected set; }

            [DataMember(Order = 5)]
            public Lessor Lessor { get; protected set; }

            [DataMember(Order = 6)]
            public Lessee Lessee { get; protected set; }

            [DataMember(Order = 7)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 8)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 9)]
            public IList<OrderEventLine> Items { get; protected set; }
        }

        [DataContract]
        public class OrderCreated : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 3)]
            public Actor Initiator { get; protected set; }

            [DataMember(Order = 4)]
            public Lessor Lessor { get; protected set; }

            [DataMember(Order = 5)]
            public Lessee Lessee { get; protected set; }

            [DataMember(Order = 6)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 7)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 8)]
            public IList<OrderEventLine> Lines { get; protected set; }
        }

        [DataContract]
        public class OrderEventLine
        {
            [DataMember(Order = 1)]
            public Guid LineId { get; set; }

            [DataMember(Order = 2)]
            public Guid ArticleId { get; set; }

            [DataMember(Order = 3)]
            public int ArticleShortId { get; set; }

            [DataMember(Order = 10)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 4)]
            public string Text { get; set; }

            [DataMember(Order = 5)]
            public decimal UnitPricePerWeek { get; set; }

            [DataMember(Order = 6)]
            public DateTime FromUtc { get; set; }

            [DataMember(Order = 7)]
            public DateTime ToUtc { get; set; }

            [DataMember(Order = 8)]
            public int Quantity { get; set; }

            [DataMember(Order = 9)]
            public decimal LineTotal { get; set; }
        }

        [DataContract]
        public class OrderItemAdded : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Actor Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 3)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 4)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 5)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 6)]
            public OrderEventLine OrderLine { get; protected set; }
        }

        [DataContract]
        public class OrderItemPeriodChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Actor Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 3)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 4)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 5)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 6)]
            public Guid OrderItemId { get; protected set; }

            [DataMember(Order = 7)]
            public Period OldPeriod { get; protected set; }

            [DataMember(Order = 8)]
            public Period NewPeriod { get; protected set; }

            [DataMember(Order = 9)]
            public OrderEventLine OrderLine { get; protected set; }
        }

        [DataContract]
        public class OrderItemQuantityChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Actor Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 3)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 4)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 5)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 6)]
            public Guid OrderItemId { get; protected set; }

            [DataMember(Order = 7)]
            public int OldQuantity { get; protected set; }

            [DataMember(Order = 8)]
            public int NewQuantity { get; protected set; }

            [DataMember(Order = 9)]
            public OrderEventLine OrderLine { get; protected set; }
        }

        [DataContract]
        public class OrderItemRemoved : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Actor Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 3)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 4)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 5)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 6)]
            public OrderEventLine OrderLine { get; protected set; }
        }

        [DataContract]
        public class OrderItemTotalChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Actor Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 3)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 4)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 5)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 6)]
            public Guid OrderItemId { get; protected set; }

            [DataMember(Order = 7)]
            public decimal OldItemTotal { get; protected set; }

            [DataMember(Order = 8)]
            public decimal NewItemTotal { get; protected set; }

            [DataMember(Order = 9)]
            public OrderEventLine OrderLine { get; protected set; }
        }

        [DataContract]
        public class OrderPlaced : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; protected set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 4)]
            public Actor Initiator { get; protected set; }

            [DataMember(Order = 5)]
            public Lessor Lessor { get; protected set; }

            [DataMember(Order = 6)]
            public Lessee Lessee { get; protected set; }

            [DataMember(Order = 7)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 8)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 9)]
            public IList<OrderEventLine> Items { get; protected set; }
        }

        [DataContract]
        public class OrderRejected : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; protected set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; protected set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; protected set; }

            [DataMember(Order = 4)]
            public Actor Manager { get; protected set; }

            [DataMember(Order = 5)]
            public Lessor Lessor { get; protected set; }

            [DataMember(Order = 6)]
            public Lessee Lessee { get; protected set; }

            [DataMember(Order = 7)]
            public int OrderStatus { get; protected set; }

            [DataMember(Order = 8)]
            public decimal OrderTotal { get; protected set; }

            [DataMember(Order = 9)]
            public IList<OrderEventLine> Items { get; protected set; }

            [DataMember(Order = 10)]
            public Actor Initiator { get; set; }
        }
    }
}