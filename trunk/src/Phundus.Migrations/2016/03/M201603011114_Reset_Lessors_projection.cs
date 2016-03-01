namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603011114)]
    public class M201603011114_Reset_Lessors_projection : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_Shop_Lessors");
            DeleteTracker("LessorsProjection");
        }
    }
}