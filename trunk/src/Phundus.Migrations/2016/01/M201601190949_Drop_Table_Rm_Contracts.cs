namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601190949)]
    public class M201601190949_Drop_Table_Rm_Contracts : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Rm_Contracts").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}