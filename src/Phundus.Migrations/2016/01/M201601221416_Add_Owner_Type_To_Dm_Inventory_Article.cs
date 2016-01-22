namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601221416)]
    public class M201601221416_Add_Owner_Type_To_Dm_Inventory_Article : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_Inventory_Article")
                .AddColumn("Owner_Type").AsInt32().NotNullable().WithDefaultValue(0);

            Execute.Sql(@"
UPDATE [Dm_Inventory_Article] SET [Owner_Type] = 1
WHERE [Owner_OwnerId] IN (SELECT [Guid] FROM [Dm_IdentityAccess_Organization]);

UPDATE [Dm_Inventory_Article] SET [Owner_Type] = 2
WHERE [Owner_OwnerId] IN (SELECT [Guid] FROM [Dm_IdentityAccess_User]);");


            Alter.Table("Dm_Inventory_Article")
                .AlterColumn("Owner_Type").AsInt32().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}