namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201407281552)]
    public class M201407281552DeleteTableSetting : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Setting");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}