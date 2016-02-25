namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602251040)]
    public class M201602251040_Add_column_Version_to_StoredEvent : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("StoredEvents").AddColumn("Version").AsInt32().NotNullable().WithDefaultValue(0);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}