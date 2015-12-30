namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512271305)]
    public class M201512271305AlterTableCartItem : MigrationBase
    {
        public override void Up()
        {
            // ¯\_(ツ)_/¯
            Delete.FromTable("CartItem").InSchema(SchemaName).AllRows();

            Alter.Table("CartItem").InSchema(SchemaName)
                .AddColumn("Article_Owner_OwnerId").AsGuid().NotNullable()
                .AddColumn("Article_Owner_Name").AsString().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}