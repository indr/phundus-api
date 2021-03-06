namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using FluentMigrator;
    using Inventory.Model;


    [Migration(201602020534)]
    public class M201602020534_Create_or_populate_ArticleCreated : EventMigrationBase
    {
        protected override void Migrate()
        {
            const string typeName = @"Phundus.Inventory.Articles.Model.ArticleCreated, Phundus.Core";
            var storedEvents = FindStoredEvents(typeName);
            var domainEvents =
                storedEvents.Select(s => Deserialize<ArticleCreated>(s)).OrderBy(p => p.OccuredOnUtc).ToList();

            var command = CreateCommand(@"SELECT TOP 1000 [Id]
      ,[Version]
      ,[CreateDate]
      ,[Name]
      ,[Brand]
      ,[PublicPrice]
      ,[Description]
      ,[Specification]
      ,[Stock]
      ,[Color]
      ,[Price_InfoCard]
      ,[Owner_OwnerId]
      ,[Owner_Name]
      ,[StoreId]
      ,[ArticleGuid]
      ,[MemberPrice]
      ,[Owner_Type]
  FROM [Dm_Inventory_Article] ORDER BY [CreateDate] ASC");

            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    var ownerGuid = reader.GetGuid(11);
                    var createdAt = reader.GetDateTime(2);
                    createdAt = createdAt.ToUniversalTime();
                    var articleGuid = reader.GetGuid(14);

                    var domainEvent = domainEvents.SingleOrDefault(p => p.OccuredOnUtc >= createdAt.AddSeconds(-5)
                                                                        && p.OccuredOnUtc <= createdAt.AddSeconds(5)
                                                                        && p.ArticleGuid == Guid.Empty);
                    var isNew = domainEvent == null;
                    if (domainEvent == null)
                        domainEvent = new ArticleCreated();

                    domainEvent.ArticleGuid = articleGuid;
                    domainEvent.ArticleId = reader.GetInt32(0);
                    domainEvent.GrossStock = reader.GetInt32(8);
                    domainEvent.Name = reader.GetString(3);
                    domainEvent.Owner = domainEvent.Owner ?? new Owner();
                    domainEvent.Owner.Name = reader.GetString(12);
                    domainEvent.Owner.OwnerId = ownerGuid;
                    domainEvent.Owner.Type = (OwnerType) reader.GetInt32(16);
                    domainEvent.StoreId = reader.GetGuid(13);

                    if (isNew)
                        InsertStoredEvent(createdAt, typeName, domainEvent);
                    else
                    {
                        UpdateStoredEvent(domainEvent.EventGuid, domainEvent);
                    }
                }
        }

        [DataContract]
        public class ArticleCreated : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public Owner Owner { get; set; }

            [DataMember(Order = 3)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 4)]
            public int ArticleId { get; set; }

            [DataMember(Order = 5)]
            public Guid ArticleGuid { get; set; }

            [DataMember(Order = 6)]
            public string Name { get; set; }

            [DataMember(Order = 7)]
            public int GrossStock { get; set; }

            [DataMember(Order = 8)]
            public decimal PublicPrice { get; set; }

            [DataMember(Order = 9)]
            public decimal? MemberPrice { get; set; }
        }

        [DataContract]
        public class Initiator
        {
            [DataMember(Order = 1)]
            public Guid InitiatorGuid { get; set; }

            [DataMember(Order = 2)]
            public string EmailAddress { get; set; }

            [DataMember(Order = 3)]
            public string FullName { get; set; }
        }

        [DataContract]
        public class Owner
        {
            [DataMember(Order = 1)]
            public virtual Guid OwnerId { get; set; }

            [DataMember(Order = 3)]
            public virtual string Name { get; set; }

            [DataMember(Order = 2)]
            public virtual OwnerType Type { get; set; }
        }
    }
}