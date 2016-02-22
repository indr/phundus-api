namespace Phundus.Migrations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602221011)]
    public class M201602221011_Generate_organization_established_events : EventMigrationBase
    {
        protected override void Migrate()
        {
            var command = CreateCommand("SELECT [Guid], [CreateDate], [Name] FROM [Dm_IdentityAccess_Organization]");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var evnt = new OrganizationEstablished();
                    evnt.Initiator = null;
                    evnt.Name = reader.GetString(2);
                    evnt.OrganizationId = reader.GetGuid(0);
                    evnt.Plan = "free";
                    evnt.OccuredOnUtc = reader.GetDateTime(1);

                    InsertStoredEvent(evnt.OccuredOnUtc, "Phundus.IdentityAccess.Organizations.Model.OrganizationEstablished, Phundus.Core", evnt);
                }
            }
        }

        [DataContract]
        public class OrganizationEstablished : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public Guid OrganizationId { get; set; }

            [DataMember(Order = 3)]
            public string Name { get; set; }

            [DataMember(Order = 4)]
            public string Plan { get; set; }
        }
    }
}