namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602080424)]
    public class M201602080424_Generate_Prices_Changed_Description_Changed_and_Specification_Changed_stored_events :
        EventMigrationBase
    {
        private const string PricesChangedTypeName =
            @"Phundus.Inventory.Articles.Model.PricesChanged, Phundus.Core";

        private const string DescriptionChangedTypeName =
            @"Phundus.Inventory.Articles.Model.DescriptionChanged, Phundus.Core";

        private const string SpecificationChangedTypeName =
            @"Phundus.Inventory.Articles.Model.SpecificationChanged, Phundus.Core";

        protected override void Migrate()
        {
            var pricesChangedEvents = FindStoredEvents(PricesChangedTypeName)
                .Select(s => Deserialize<PricesChanged>(s.Serialization)).ToList();
            var descriptionChangedEvents = FindStoredEvents(DescriptionChangedTypeName)
                .Select(s => Deserialize<DescriptionChanged>(s.Serialization)).ToList();
            var specificationChangedÊvents = FindStoredEvents(SpecificationChangedTypeName)
                .Select(s => Deserialize<SpecificationChanged>(s.Serialization)).ToList();

            var command = CreateCommand(@"SELECT [Id]
      ,[ArticleGuid]
      ,[CreateDate]
      ,[PublicPrice]
      ,[MemberPrice]      
      ,[Description]
      ,[Specification]
      ,[Owner_OwnerId]
  FROM [Dm_Inventory_Article]");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var articleShortId = reader.GetInt32(0);
                    var articleGuid = reader.GetGuid(1);
                    var createdAtUtc = reader.GetDateTime(2).ToUniversalTime().AddSeconds(1);
                    var publicPrice = reader.GetDecimal(3);
                    decimal? memberPrice = null;
                    if (!reader.IsDBNull(4))
                        memberPrice = reader.GetDecimal(4);
                    var description = reader.IsDBNull(5) ? null : reader.GetString(5);
                    var specification = reader.IsDBNull(6) ? null : reader.GetString(6);
                    var ownerId = reader.GetGuid(7);
                    if (pricesChangedEvents.Find(p => p.ArticleGuid == articleGuid) == null)
                    {
                        InsertStoredEvent(createdAtUtc, PricesChangedTypeName, new PricesChanged
                        {
                            ArticleGuid = articleGuid,
                            ArticleIntegralId = articleShortId,
                            Initiator = null,
                            OwnerId = ownerId,
                            MemberPrice = memberPrice,
                            PublicPrice = publicPrice
                        });
                    }

                    if (!String.IsNullOrWhiteSpace(description) &&
                        descriptionChangedEvents.Find(p => p.ArticleGuid == articleGuid) == null)
                    {
                        InsertStoredEvent(createdAtUtc, DescriptionChangedTypeName, new DescriptionChanged
                        {
                            ArticleGuid = articleGuid,
                            ArticleIntegralId = articleShortId,
                            Initiator = null,
                            OwnerId = ownerId,
                            Description = specification
                        });
                    }

                    if (!String.IsNullOrWhiteSpace(specification) &&
                        specificationChangedÊvents.Find(p => p.ArticleGuid == articleGuid) == null)
                    {
                        InsertStoredEvent(createdAtUtc, SpecificationChangedTypeName, new SpecificationChanged
                        {
                            ArticleGuid = articleGuid,
                            ArticleIntegralId = articleShortId,
                            Initiator = null,
                            OwnerId = ownerId,
                            Specification = specification
                        });
                    }
                }
            }
        }

        [DataContract]
        public class DescriptionChanged
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public int ArticleIntegralId { get; set; }

            [DataMember(Order = 3)]
            public Guid ArticleGuid { get; set; }

            [DataMember(Order = 4)]
            public Guid OwnerId { get; set; }

            [DataMember(Order = 5)]
            public string Description { get; set; }
        }

        [DataContract]
        public class PricesChanged
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public int ArticleIntegralId { get; set; }

            [DataMember(Order = 3)]
            public Guid ArticleGuid { get; set; }

            [DataMember(Order = 4)]
            public decimal PublicPrice { get; set; }

            [DataMember(Order = 5)]
            public decimal? MemberPrice { get; set; }

            [DataMember(Order = 6)]
            public Guid OwnerId { get; set; }
        }

        [DataContract]
        public class SpecificationChanged
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public int ArticleIntegralId { get; set; }

            [DataMember(Order = 3)]
            public Guid ArticleGuid { get; set; }

            [DataMember(Order = 4)]
            public Guid OwnerId { get; set; }

            [DataMember(Order = 5)]
            public string Specification { get; set; }
        }
    }
}