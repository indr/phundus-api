namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201602291228)]
    public class M201602291228_Reset_OrganizationProjection : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_IdentityAccess_Organizations");
            DeleteTracker("OrganizationProjection");
        }

        public override void Down()
        {            
        }
    }
}