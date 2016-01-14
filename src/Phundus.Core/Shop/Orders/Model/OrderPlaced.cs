namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(int orderId, LessorId lessorId, IList<Item> items)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            OrderId = orderId;
            LessorId = lessorId.Id;
            //Items = items ?? new Item[0];
        }

        protected OrderPlaced()
        {
        }

        [DataMember(Order = 1)]
        public int OrderId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid LessorId { get; set; }

        //[DataMember(Order = 2)]
        //public IList<Item> Items { get; protected set; }


        public class Item
        {
        }
    }
}