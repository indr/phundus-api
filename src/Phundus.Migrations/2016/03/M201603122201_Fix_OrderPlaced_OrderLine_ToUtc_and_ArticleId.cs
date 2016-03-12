namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201603122201)]
    public class M201603122201_Fix_OrderPlaced_OrderLine_ToUtc_and_ArticleId : EventMigrationBase
    {
        protected override void Migrate()
        {
            var events = FindStoredEvents<OrderPlaced>(@"Phundus.Shop.Orders.Model.OrderPlaced, Phundus.Core")
                .Where(p => p.OrderShortId == 10020 || p.OrderShortId == 10154 || p.OrderShortId == 10206 || p.OrderShortId == 10213);            

            foreach (var e in events)
            {
                var changed = false;
                foreach (var line in e.Items)
                {
                    if (line.ToUtc < line.FromUtc)
                    {
                        line.ToUtc = line.FromUtc;
                        changed = true;
                    }
                    if (line.ArticleId == Guid.Empty)
                    {
                        line.ArticleId = DeletedArticleIdMap[line.ArticleShortId];
                        changed = true;
                    }
                }

                if (changed)
                    UpdateStoredEvent(e.EventGuid, e);
            }
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
            public Initiator Initiator { get; protected set; }

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
    }
}