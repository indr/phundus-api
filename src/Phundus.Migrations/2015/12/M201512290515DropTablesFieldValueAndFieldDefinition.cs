namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512290515)]
    public class M201512290515DropTablesFieldValueAndFieldDefinition : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("FieldValue").InSchema(SchemaName);
            Delete.Table("FieldDefinition").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}