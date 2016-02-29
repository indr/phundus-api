namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602291001)]
    public class M201602291001_Reset_OrganizationProjection : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_IdentityAccess_Organization");
            DeleteTable("Es_IdentityAccess_Organizations");
            DeleteTracker("OrganizationProjection");
            DeleteTracker("OrganizationsProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}