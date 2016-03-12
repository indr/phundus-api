namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201603030653)]
    public class M201603030653_Add_columns_to_ProcessedNotificationTracker : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("ProcessedNotificationTracker")
                .AddColumn("MostRecentProcessedAtUtc").AsDateTime().Nullable()
                .AddColumn("ErrorMessage").AsString().Nullable()
                .AddColumn("ErrorAtUtc").AsDateTime().Nullable();
        }
    }
}