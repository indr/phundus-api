namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Profile("ResetDatabase")]
    public class ResetDatabaseProfile : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("Dm_IdentityAccess_Account").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_Application").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_Membership").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_Organization").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_User").InSchema(SchemaName).AllRows();

            Delete.FromTable("Dm_Inventory_ArticleFile").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Inventory_Article").InSchema(SchemaName).AllRows();

            Delete.FromTable("Dm_Shop_CartItem").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Shop_Cart").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Shop_OrderItem").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Shop_Order").InSchema(SchemaName).AllRows();

            DropTable("Es_Shop_Item_Files");
            DropTable("Es_Shop_Item_Documents");
            DropTable("Es_Shop_Item_Images");            
            DropTable("Es_Shop_Item");
            DropTable("Es_Shop_Items_Popularity");
            DropTable("Es_Shop_Items");
            DropTable("Es_Shop_ProductDetails_Document");
            DropTable("Es_Shop_ProductDetails_Image");
            DropTable("Es_Shop_ProductDetails_Tags");
            DropTable("Es_Shop_ProductDetails");
            DropTable("Es_Shop_ProductList_Popularity");
            DropTable("Es_Shop_ProductList_Tags");
            DropTable("Es_Shop_ProductList");
            DropTable("Es_Shop_Orders_Lines");
            DropTable("Es_Shop_Orders");

            DropTable("Es_Inventory_Articles_Tags");
            DropTable("Es_Inventory_Articles");

            DeleteAllRowsFromTableWithPrefix("Es_");

            Delete.FromTable("ProcessedNotificationTracker").InSchema(SchemaName).AllRows();
            Delete.FromTable("StoredEvents").InSchema(SchemaName).AllRows();
            
            Reseed("StoredEvents", 0);
        }

        private void DropTable(string tableName)
        {
            if (Schema.Table(tableName).Exists())
                Delete.Table(tableName);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/536350/drop-all-the-tables-stored-procedures-triggers-constraints-and-all-the-depend
        /// </summary>
        /// <param name="tableNamePrefix"></param>
        private void DeleteAllRowsFromTableWithPrefix(string tableNamePrefix)
        {
            Execute.Sql(String.Format(@"
/* Drop all tables */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 AND [name] LIKE '{0}%' ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP TABLE [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Table: ' + @name
    SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 AND [name] > @name AND [name] LIKE '{0}%' ORDER BY [name])
END
GO", tableNamePrefix));
        }

        private void DeleteAllRowsIfTableExists(string tableName)
        {
            if (Schema.Table(tableName).Exists())
                Delete.FromTable(tableName).AllRows();
        }

        public override void Down()
        {
            // Nothing to do here...
        }
    }
}