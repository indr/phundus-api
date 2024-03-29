﻿namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201204211635)]
    public class M0001_CreateNHibernateUniqueKeyTable : MigrationBase
    {
        private const string TableName = "hibernate_unique_key";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("next_hi").AsInt32();

            Execute.Sql("insert into hibernate_unique_key values ( 1 );");
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}