namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603122019)]
    public class M201603122019_Alter_columns_on_ProcessedNotificationTracker : MigrationBase
    {
        public override void Up()
        {            
            Alter.Table("ProcessedNotificationTracker").AlterColumn("ErrorMessage").AsMaxString().Nullable();
        }
    }
}