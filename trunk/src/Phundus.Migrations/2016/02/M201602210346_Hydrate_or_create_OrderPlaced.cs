namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;
    using Shop.Model;
    using Shop.Orders.Model;

    [Migration(201602210346)]
    public class M201602210346_Hydrate_or_create_OrderPlaced : EventMigrationBase
    {
       private IList<ListItem> _items = new List<ListItem>(); 

        public class ListItem
        {
            public int OrderShortId { get; set; }
            public DateTime CreatedAtUtc { get; set; }
            public bool IsNew { get; set; }
            public OrderPlaced Evnt { get; set; }
        }

        protected override void Migrate()
        {
            const string typeName = @"Phundus.Shop.Orders.Model.OrderPlaced, Phundus.Core";
            var storedEvents = FindStoredEvents(typeName);
            var domainEvents = storedEvents.Select(s => Deserialize<OrderPlaced>(s)).ToList();

            var command = CreateCommand(@"SELECT [Id]
      ,[Version]
      ,[Status]
      ,[Borrower_FirstName]
      ,[Borrower_LastName]
      ,[Borrower_EmailAddress]
      ,[Borrower_Street]
      ,[Borrower_Postcode]
      ,[Borrower_City]
      ,[Borrower_MobilePhoneNumber]
      ,[Borrower_MemberNumber]
      ,[CreatedUtc]
      ,[ModifiedUtc]
      ,[Lessor_LessorId]
      ,[Lessor_Name]
      ,[OrderGuid]
      ,[Lessee_LesseeGuid]
  FROM [Dm_Shop_Order]");

          

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var shortOrderId = reader.GetInt32(0);

                    var evnt = domainEvents.SingleOrDefault(p => p.OrderShortId == shortOrderId);
                    var isNew = false;
                    if (evnt == null)
                    {
                        isNew = true;
                        evnt = new OrderPlaced();
                        evnt.OrderShortId = shortOrderId;
                    }

                    var createdAtUtc = reader.GetDateTime(11);

                    var lessorId = reader.GetGuid(13);
                    var lessorName = reader.GetString(14);

                    var lesseeId = reader.GetGuid(16);
                    var lesseeEmailAddress = reader.GetString(5);
                    var lesseeFullName = reader.GetString(3) + " " + reader.GetString(4);
                    
                    evnt.LessorId = lessorId;
                    evnt.OrderId = reader.GetGuid(15);
                    evnt.Initiator = new Initiator(new InitiatorId(lesseeId), lesseeEmailAddress, lesseeFullName);
                    evnt.Lessor = new Lessor(new LessorId(lessorId), lessorName, false);
                    evnt.Lessee = new Lessee(new LesseeId(lesseeId), reader.GetString(3), reader.GetString(4),
                        reader.IsDBNull(6) ? "" : reader.GetString(6),
                        reader.IsDBNull(7) ? "" : reader.GetString(7),
                        reader.IsDBNull(8) ? "" : reader.GetString(8),
                        lesseeEmailAddress,
                        reader.IsDBNull(9) ? "" : reader.GetString(9),
                        reader.IsDBNull(10)? "" : reader.GetString(10));
                    evnt.OrderStatus = 1;
                    evnt.Items = new List<OrderEventItem>();


                    _items.Add(new ListItem
                    {
                        CreatedAtUtc = createdAtUtc,
                        Evnt = evnt,
                        IsNew = isNew,
                        OrderShortId = shortOrderId
                    });
                }
            }

            foreach (var each in _items)
            {
                var selectItemsCmd = CreateCommand(@"SELECT [Version]
      ,[OrderId]
      ,[Amount]
      ,[ArticleId]
      ,[Id]
      ,[FromUtc]
      ,[ToUtc]
      ,[UnitPrice]
      ,[Text]
      ,[Total]
      ,[OrderItemGuid]
      ,[ArticleGuid]
  FROM [Dm_Shop_OrderItem]
  WHERE [OrderId] = @OrderId");

                var param = selectItemsCmd.CreateParameter();
                param.ParameterName = "@OrderId";
                param.Value = each.OrderShortId;
                selectItemsCmd.Parameters.Add(param);

                using (var reader = selectItemsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var articleId = reader.GetGuid(11);

                        var itemTotal = reader.GetDecimal(9);
                        var item = new OrderEventItem(
                            reader.GetGuid(10), articleId,
                            new ArticleShortId(reader.GetInt32(3)), reader.GetString(8),
                            reader.GetDecimal(7), reader.GetDateTime(5), reader.GetDateTime(6),
                            reader.GetInt32(2), itemTotal);

                        var evnt = each.Evnt;
                        evnt.Items.Add(item);
                        evnt.OrderTotal += itemTotal;
                    }
                }

                if (each.IsNew)
                {
                    InsertStoredEvent(each.CreatedAtUtc, typeName, each.Evnt);
                }
                else
                {
                    UpdateStoredEvent(each.Evnt.EventGuid, each.Evnt);
                }
            }            
        }

        [DataContract]
        public class OrderPlaced : MigratingDomainEvent
        {
            public OrderPlaced(Initiator initiator, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee,
                int orderStatus, decimal orderTotal, IList<OrderEventItem> items)
            {
                if (initiator == null) throw new ArgumentNullException("initiator");
                if (orderId == null) throw new ArgumentNullException("orderId");
                if (orderShortId == null) throw new ArgumentNullException("orderShortId");
                if (lessor == null) throw new ArgumentNullException("lessor");
                if (lessee == null) throw new ArgumentNullException("lessee");
                if (items == null) throw new ArgumentNullException("items");
                Initiator = initiator;
                OrderId = orderId.Id;
                OrderShortId = orderShortId.Id;
                Lessor = lessor;
                Lessee = lessee;
                OrderStatus = orderStatus;
                OrderTotal = orderTotal;
                LessorId = lessor.LessorId.Id;
                Items = items;
            }

            public OrderPlaced()
            {
            }

            [DataMember(Order = 1)]
            public int OrderShortId { get; set; }

            [DataMember(Order = 2)]
            public Guid LessorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrderId { get; set; }

            [DataMember(Order = 4)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 5)]
            public Lessor Lessor { get; set; }

            [DataMember(Order = 6)]
            public Lessee Lessee { get; set; }

            [DataMember(Order = 7)]
            public int OrderStatus { get; set; }

            [DataMember(Order = 8)]
            public decimal OrderTotal { get; set; }

            [DataMember(Order = 9)]
            public IList<OrderEventItem> Items { get; set; }
        }

        [DataContract]
        public class OrderEventItem
        {
            public OrderEventItem(Guid itemId, Guid articleId, ArticleShortId articleShortId, string text,
                decimal unitPricePerWeek, DateTime fromUtc, DateTime toUtc, int quantity, decimal itemTotal)
            {
                if (articleId == null) throw new ArgumentNullException("articleId");
                if (articleShortId == null) throw new ArgumentNullException("articleShortId");
                if (text == null) throw new ArgumentNullException("text");
                ItemId = itemId;
                ArticleId = articleId;
                ArticleShortId = articleShortId.Id;
                Text = text;
                UnitPricePerWeek = unitPricePerWeek;
                FromUtc = fromUtc;
                ToUtc = toUtc;
                Quantity = quantity;
                ItemTotal = itemTotal;
            }

            protected OrderEventItem()
            {
            }

            [DataMember(Order = 1)]
            public Guid ItemId { get; set; }

            [DataMember(Order = 2)]
            public Guid ArticleId { get; set; }

            [DataMember(Order = 3)]
            public int ArticleShortId { get; set; }

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
            public decimal ItemTotal { get; set; }
        }
    }
}