namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201603130600)]
    public class M201603130600_Insert_stored_events_ArticleCreated_and_ArticleDeleted : EventMigrationBase
    {
        public enum OwnerType
        {
            Unknown,
            Organization,
            User
        }

        private readonly Guid _initiatorId = new Guid("D76CA91A-DAD5-4662-8AB1-9F612FE5249B");
            // lukas.mueller.me@gmail.com

        private readonly Guid _ownerId = new Guid("428D069D-1183-4643-BECF-276A9BC523BE"); // piNuts
        private readonly Guid _storeId = new Guid("8CFCC30B-5577-441A-A176-FFB449856F71"); // piNuts default store
        private Initiator _initiator;
        private Owner _owner;

        protected override void Migrate()
        {            
            if (!FindOrganization(_ownerId))
                return;

            _initiator = new Initiator(new InitiatorId(_initiatorId), "lukas.mueller.me@gmail.com", "Lukas Müller");
            _owner = new Owner(new OwnerId(_ownerId), "piNuts", OwnerType.Organization);

            CreateAndAddCommand("SET IDENTITY_INSERT [StoredEvents] ON");
            InsertArticleCreated();
            CreateAndAddCommand("SET IDENTITY_INSERT [StoredEvents] OFF");
            InsertArticleDeleted();
        }

        private bool FindOrganization(Guid ownerId)
        {
            var command = CreateCommand(@"
SELECT COUNT(*) FROM [Dm_IdentityAccess_Organization] WHERE [Guid] = '" + ownerId + "'");

            return (int) command.ExecuteScalar() > 0;
        }

        private void InsertArticleCreated()
        {
            var names = new Dictionary<int, string>
            {
                {10111, "Lieferwagen"},
                {10110, "Veloanhänger"}
            };
            var publicPrices = new Dictionary<int, decimal>
            {
                {10111, 650},
                {10110, 50}
            };
            long eventId = 0;
            var occuredOnUtc = new DateTime(2014, 12, 19, 4, 0, 0, DateTimeKind.Utc);
            foreach (var each in DeletedArticleIdMap)
            {
                eventId = FindLowestUnusedEventId(eventId);
                var e = new ArticleCreated(_initiator, _owner, new StoreId(_storeId), null, new ArticleShortId(each.Key),
                    new ArticleId(each.Value), names[each.Key], 1, publicPrices[each.Key], null);
                InsertEvent(eventId, e, "Phundus.Inventory.Articles.Model.ArticleCreated, Phundus.Core", occuredOnUtc);
                occuredOnUtc = occuredOnUtc.AddHours(1);
            }
        }

        private void InsertArticleDeleted()
        {
            var occuredOnUtc = new DateTime(2015, 12, 17, 4, 0, 0, DateTimeKind.Utc);
            foreach (var each in DeletedArticleIdMap)
            {
                var e = new ArticleDeleted(_initiator, new ArticleShortId(each.Key), new ArticleId(each.Value),
                    new OwnerId(_ownerId));
                InsertEvent(e, "Phundus.Inventory.Articles.Model.ArticleDeleted, Phundus.Core", occuredOnUtc);
                occuredOnUtc = occuredOnUtc.AddHours(1);
            }
        }


        private void InsertEvent(long eventId, MigratingDomainEvent migratingDomainEvent, string typeName,
            DateTime occuredOnUtc)
        {
            var command = CreateAndAddCommand(@"
INSERT INTO [StoredEvents]
           ([EventId]
           ,[EventGuid]
           ,[TypeName]
           ,[OccuredOnUtc]
           ,[AggregateId]
           ,[Serialization]
           ,[Version])
     VALUES (@EventId, @EventGuid, @TypeName, @OccuredOnUtc, @AggregateId, @Serialization, 0)");

            command.Parameters.Add(new SqlParameter("@EventId", eventId));
            command.Parameters.Add(new SqlParameter("@EventGuid", Guid.NewGuid()));
            command.Parameters.Add(new SqlParameter("@TypeName", typeName));
            command.Parameters.Add(new SqlParameter("@OccuredOnUtc", occuredOnUtc));
            command.Parameters.Add(new SqlParameter("@AggregateId", Guid.Empty));
            command.Parameters.Add(new SqlParameter("@Serialization", Serialize(migratingDomainEvent)));            
        }

        private void InsertEvent(MigratingDomainEvent migratingDomainEvent, string typeName, DateTime occuredOnUtc)
        {
            var command = CreateAndAddCommand(@"
INSERT INTO [StoredEvents]
           ([EventGuid]
           ,[TypeName]
           ,[OccuredOnUtc]
           ,[AggregateId]
           ,[Serialization]
           ,[Version])
     VALUES (@EventGuid, @TypeName, @OccuredOnUtc, @AggregateId, @Serialization, 0)");

            command.Parameters.Add(new SqlParameter("@EventGuid", Guid.NewGuid()));
            command.Parameters.Add(new SqlParameter("@TypeName", typeName));
            command.Parameters.Add(new SqlParameter("@OccuredOnUtc", occuredOnUtc));
            command.Parameters.Add(new SqlParameter("@AggregateId", Guid.Empty));
            command.Parameters.Add(new SqlParameter("@Serialization", Serialize(migratingDomainEvent)));            
        }

        private long FindLowestUnusedEventId(long after)
        {
            var command = CreateCommand(@"
SELECT TOP 1 r.EventId + 1 AS LowestUnused
FROM [StoredEvents] l
RIGHT JOIN [StoredEvents] r ON l.EventId = r.EventId + 1
WHERE l.EventId IS NULL AND r.EventId + 1 > " + after);
            var result = (long?) command.ExecuteScalar();
            return result ?? 1;
        }

        [DataContract]
        public class ArticleCreated : MigratingDomainEvent
        {
            public ArticleCreated(Initiator initiator, Owner owner, StoreId storeId, string storeName,
                ArticleShortId articleShortId,
                ArticleId articleId, string name, int grossStock, decimal publicPrice, decimal? memberPrice)
            {
                if (initiator == null) throw new ArgumentNullException("initiator");
                if (owner == null) throw new ArgumentNullException("owner");
                if (storeId == null) throw new ArgumentNullException("storeId");
                if (articleShortId == null) throw new ArgumentNullException("articleShortId");
                if (articleId == null) throw new ArgumentNullException("articleId");
                if (name == null) throw new ArgumentNullException("name");

                Initiator = initiator;
                Owner = owner;
                StoreName = storeName;
                StoreId = storeId.Id;
                ArticleShortId = articleShortId.Id;
                ArticleId = articleId.Id;
                Name = name;
                GrossStock = grossStock;
                PublicPrice = publicPrice;
                MemberPrice = memberPrice;
            }

            protected ArticleCreated()
            {
            }

            [DataMember(Order = 1)]
            public Initiator Initiator { get; protected set; }

            [DataMember(Order = 2)]
            public Owner Owner { get; protected set; }

            [DataMember(Order = 3)]
            public Guid StoreId { get; protected set; }

            [DataMember(Order = 10)]
            public string StoreName { get; protected set; }

            [DataMember(Order = 4)]
            public int ArticleShortId { get; protected set; }

            [DataMember(Order = 5)]
            public Guid ArticleId { get; protected set; }

            [DataMember(Order = 6)]
            public string Name { get; protected set; }

            [DataMember(Order = 7)]
            public int GrossStock { get; protected set; }

            [DataMember(Order = 8)]
            public decimal PublicPrice { get; protected set; }

            [DataMember(Order = 9)]
            public decimal? MemberPrice { get; protected set; }
        }

        [DataContract]
        public class ArticleDeleted : MigratingDomainEvent
        {
            public ArticleDeleted(Initiator initiator, ArticleShortId articleShortId, ArticleId articleId,
                OwnerId ownerId)
            {
                if (initiator == null) throw new ArgumentNullException("initiator");
                if (articleShortId == null) throw new ArgumentNullException("articleShortId");
                if (articleId == null) throw new ArgumentNullException("articleId");
                if (ownerId == null) throw new ArgumentNullException("ownerId");

                Initiator = initiator;
                ArticleShortId = articleShortId.Id;
                ArticleId = articleId.Id;
                OwnerId = ownerId.Id;
            }

            protected ArticleDeleted()
            {
            }

            [DataMember(Order = 1)]
            public Initiator Initiator { get; protected set; }

            [DataMember(Order = 2)]
            public int ArticleShortId { get; protected set; }

            [DataMember(Order = 3)]
            public Guid ArticleId { get; protected set; }

            [DataMember(Order = 4)]
            public Guid OwnerId { get; protected set; }
        }

        [DataContract]
        public class Initiator
        {
            public Initiator(InitiatorId initiatorId, string emailAddress, string fullName)
            {
                if (initiatorId == null) throw new ArgumentNullException("initiatorId");
                if (fullName == null) throw new ArgumentNullException("fullName");
                if (emailAddress == null) throw new ArgumentNullException("emailAddress");

                InitiatorId = initiatorId;
                EmailAddress = emailAddress;
                FullName = fullName;
            }

            protected Initiator()
            {
            }

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
        public class Owner
        {
            private string _name;
            private OwnerId _ownerId;
            private OwnerType _type;

            public Owner(OwnerId ownerId, string name, OwnerType type)
            {
                if (ownerId == null) throw new ArgumentNullException("ownerId");
                if (name == null) throw new ArgumentNullException("name");
                if (type == OwnerType.Unknown) throw new ArgumentException("Owner type must not be unknown.", "type");

                _ownerId = ownerId;
                _name = name;
                _type = type;
            }

            protected Owner()
            {
            }

            public virtual OwnerId OwnerId
            {
                get { return _ownerId; }
                protected set { _ownerId = value; }
            }

            [DataMember(Order = 4)]
            protected virtual Guid OwnerGuid
            {
                get { return OwnerId.Id; }
                set { OwnerId = new OwnerId(value); }
            }

            [DataMember(Order = 2)]
            public virtual OwnerType Type
            {
                get { return _type; }
                protected set { _type = value; }
            }

            [DataMember(Order = 3)]
            public virtual string Name
            {
                get { return _name; }
                protected set { _name = value; }
            }
        }
    }
}