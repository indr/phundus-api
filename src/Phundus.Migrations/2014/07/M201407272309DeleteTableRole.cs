namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201407272309)]
    public class M201407272309DeleteTableRole : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("FkUserToRole").OnTable("User");
            Delete.Table("Role");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}