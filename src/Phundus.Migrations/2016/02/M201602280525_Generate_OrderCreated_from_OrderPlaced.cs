namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602280525)]
    public class M201602280525_Generate_OrderCreated_from_OrderPlaced : EventMigrationBase
    {
        private const string OrderPlacedTypeName = "Phundus.Shop.Orders.Model.OrderPlaced, Phundus.Core";
        private const string OrderCreatedTypeName = "Phundus.Shop.Orders.Model.OrderCreated, Phundus.Core";

        protected override void Migrate()
        {
            var orderPlacedEvents = FindStoredEvents<OrderPlaced>(OrderPlacedTypeName);
            var orderCreatedEvents = FindStoredEvents<OrderCreated>(OrderCreatedTypeName);

            foreach (var placed in orderPlacedEvents)
            {
                var created = orderCreatedEvents.SingleOrDefault(p => p.OrderId == placed.OrderId);
                if (created != null)
                    continue;

                created = new OrderCreated();
                created.OrderId = placed.OrderId;
                created.OrderShortId = placed.OrderShortId;
                created.OrderStatus = placed.OrderStatus;
                created.OrderTotal = placed.OrderTotal;
                created.Initiator = placed.Initiator;
                created.Lessee = placed.Lessee;
                created.Lessor = placed.Lessor;
                created.Lines = placed.Items;

                InsertStoredEvent(placed.OccuredOnUtc, OrderPlacedTypeName, placed);
                UpdateStoredEvent(placed.EventGuid, created, created.OrderId, OrderCreatedTypeName);                
            }            
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
            public IList<OrderEventLine> Items { get; set; }
        }

        [DataContract]
        public class OrderCreated : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrderShortId { get; set; }

            [DataMember(Order = 2)]
            public Guid OrderId { get; set; }

            [DataMember(Order = 3)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 4)]
            public Lessor Lessor { get; set; }

            [DataMember(Order = 5)]
            public Lessee Lessee { get; set; }

            [DataMember(Order = 6)]
            public int OrderStatus { get; set; }

            [DataMember(Order = 7)]
            public decimal OrderTotal { get; set; }

            [DataMember(Order = 8)]
            public IList<OrderEventLine> Lines { get; set; }
        }

        [DataContract]
        public class Initiator 
        {
            public InitiatorId InitiatorId { get; protected set; }

            [DataMember(Order = 1)]
            protected Guid InitiatorGuid
            {
                get { return InitiatorId.Id; }
                set { InitiatorId = new InitiatorId(value); }
            }

            [DataMember(Order = 2)]
            public string EmailAddress { get; protected set; }

            [DataMember(Order = 3)]
            public string FullName { get; protected set; }

        }

        [DataContract]
        public class Lessor
        {
            private bool _doesPublicRental;
            private LessorId _lessorId;
            private string _name;

            public virtual LessorId LessorId
            {
                get { return _lessorId; }
                protected set { _lessorId = value; }
            }

            [DataMember(Order = 1)]
            protected virtual Guid LessorGuid
            {
                get { return LessorId.Id; }
                set { LessorId = new LessorId(value); }
            }

            [DataMember(Order = 2)]
            public virtual string Name
            {
                get { return _name; }
                protected set { _name = value; }
            }

            [DataMember(Order = 3)]
            public virtual bool DoesPublicRental
            {
                get { return _doesPublicRental; }
                protected set { _doesPublicRental = value; }
            }
        }

        [DataContract]
        public class Lessee 
        {
            private string _city;
            private string _emailAddress;
            private string _firstName;
            private string _lastName;
            private LesseeId _lesseeId;
            private string _memberNumber;
            private string _phoneNumber;
            private string _postcode;
            private string _street;

            public virtual LesseeId LesseeId
            {
                get { return _lesseeId; }
                protected set { _lesseeId = value; }
            }

            [DataMember(Order = 1)]
            protected virtual Guid LesseeGuid
            {
                get { return LesseeId.Id; }
                set { LesseeId = new LesseeId(value); }
            }

            [DataMember(Order = 2)]
            public virtual string FirstName
            {
                get { return _firstName; }
                protected set { _firstName = value; }
            }

            [DataMember(Order = 3)]
            public virtual string LastName
            {
                get { return _lastName; }
                protected set { _lastName = value; }
            }

            [DataMember(Order = 4)]
            public virtual string Street
            {
                get { return _street; }
                protected set { _street = value; }
            }

            [DataMember(Order = 5)]
            public virtual string Postcode
            {
                get { return _postcode; }
                protected set { _postcode = value; }
            }

            [DataMember(Order = 6)]
            public virtual string City
            {
                get { return _city; }
                protected set { _city = value; }
            }

            [DataMember(Order = 7)]
            public virtual string EmailAddress
            {
                get { return _emailAddress; }
                protected set { _emailAddress = value; }
            }

            [DataMember(Order = 8)]
            public virtual string PhoneNumber
            {
                get { return _phoneNumber; }
                protected set { _phoneNumber = value; }
            }

            [DataMember(Order = 9)]
            public virtual string MemberNumber
            {
                get { return _memberNumber; }
                protected set { _memberNumber = value; }
            }

            public virtual string FullName
            {
                get { return FirstName + " " + LastName; }
            }
        }

        [DataContract]
        public class OrderEventLine
        {
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
            public decimal LineTotal { get; set; }

            public Period Period
            {
                get { return new Period(FromUtc, ToUtc); }
            }

        }
    }
    
}