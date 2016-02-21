namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602210725)]
    public class M201602210725_Regenerate_Description_Changed :
        EventMigrationBase
    {
        private const string DescriptionChangedTypeName =
            @"Phundus.Inventory.Articles.Model.DescriptionChanged, Phundus.Core";

        
        protected override void Migrate()
        {
            var descriptionChangedEvents = FindStoredEvents(DescriptionChangedTypeName).OrderBy(p => p.OccuredOnUtc)
                .Select(s => Deserialize<DescriptionChanged>(s)).ToList();
        
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
                    var articleGuid = reader.GetGuid(1);
                    var description = reader.IsDBNull(5) ? null : reader.GetString(5);

                    if (!String.IsNullOrWhiteSpace(description))
                    {
                        var evnt = descriptionChangedEvents.Last(p => p.ArticleGuid == articleGuid);
                        evnt.Description = description;
                        UpdateSerialization(evnt.EventGuid, evnt);
                    }
                }
            }
        }

        [DataContract]
        public class DescriptionChanged : MigratingDomainEvent
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
    }
}