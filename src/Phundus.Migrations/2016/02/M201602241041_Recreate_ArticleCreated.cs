namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602241042)]
    public class M201602241042_Reserialize_ArticleCreated : EventMigrationBase
    {
        public class OwnerListItem
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Type { get; set; }
        }

        protected override void Migrate()
        {
            const string typeName = @"Phundus.Inventory.Articles.Model.ArticleCreated, Phundus.Core";
            var storedEvents = FindStoredEvents(typeName);
            var domainEvents = storedEvents.Select(s => s.Deserialize<ArticleCreated>()).OrderBy(p => p.OccuredOnUtc).ToList();

            var owners = new List<OwnerListItem>();

            var command = CreateCommand(@"
SELECT [LessorGuid]
      ,[LessorType]
      ,[Name]
  FROM [View_Shop_Lessors]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetGuid(0);
                    if (id == new Guid("F99B2D27-E92F-4784-AE95-28EC88B59C8D"))
                        continue;
                    owners.Add(new OwnerListItem
                    {
                        Id = id,
                        Name = reader.GetString(2),
                        Type = reader.GetInt32(1)
                    });
                }
            }

            foreach (var e in domainEvents)
            {
                if (e.Owner.OwnerId != Guid.Empty)
                    continue;
                var owner = owners.FindAll(p => p.Name == e.Owner.Name);
                if (owner.Count > 1)
                    throw new Exception("Multiple owners with name " + e.Owner.Name);
                e.Owner.OwnerId = owner.First().Id;

                UpdateStoredEvent(e.EventGuid, e);  
            }
        }

        [DataContract]
        public class ArticleCreated : DomainEvent
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
        public class Owner
        {
            [DataMember(Order = 4)]
            public virtual Guid OwnerId { get; set; }

            [DataMember(Order = 3)]
            public virtual string Name { get; set; }

            [DataMember(Order = 2)]
            public virtual OwnerType Type { get; set; }
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

        public enum OwnerType
        {
            Unknown,
            Organization,
            User
        }
    }
}