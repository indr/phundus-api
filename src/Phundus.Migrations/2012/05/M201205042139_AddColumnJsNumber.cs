﻿namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201205042139)]
    public class M201205042139_AddColumnJsNumber : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("User").InSchema(SchemaName).AddColumn("JsNumber").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("JsNumber").FromTable("User").InSchema(SchemaName);
        }
    }
}