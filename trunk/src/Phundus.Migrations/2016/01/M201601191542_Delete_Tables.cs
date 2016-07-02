namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601191542)]
    public class M201601191542_Delete_Unused_Tables : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Rm_Relationships");
            if (Schema.Table("Rm_EventLog").Exists())
                Rename.Table("Rm_EventLog").To("Es_Dashboard_EventLog");

            if (Schema.Table("model_store").Exists())
                Delete.Table("model_store");
            if (Schema.Table("SequelizeMeta").Exists())
                Delete.Table("SequelizeMeta");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}