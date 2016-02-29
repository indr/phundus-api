namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using FluentMigrator;

    [Migration(201602291038)]
    public class M201602291038_Delete_OrganizationEstablished_Reserved : EventMigrationBase
    {
        private const string TypeName = "Phundus.IdentityAccess.Organizations.Model.OrganizationEstablished, Phundus.Core";

        protected override void Migrate()
        {
            var reservedId = new Guid("1E2311AD-2340-4AB1-BE0E-54DA9658FBD7");
            var se = FindStoredEvents<OrganizationEstablished>(TypeName)
                .SingleOrDefault(p => p.OrganizationId == reservedId);

            if (se == null)
                return;
            DeleteStoredEvent(se.EventGuid);
        }

        public override void Up()
        {
            base.Up();

            DeleteTableAndResetTracker("Es_IdentityAccess_Organizations", "OrganizationProjection");
        }

        [DataContract]
        public class OrganizationEstablished : MigratingDomainEvent
        {
            [DataMember(Order = 2)]
            public Guid OrganizationId { get; set; }

            [DataMember(Order = 3)]
            public string Name { get; set; }

            [DataMember(Order = 4)]
            public string Plan { get; set; }
        }
    }
}