namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601110544)]
    public class M201601110544AlterTableNames : MigrationBase
    {
        public override void Up()
        {
            Rename.Table("Article").To("Dm_Inventory_Article");
            Rename.Table("Cart").To("Dm_Shop_Cart");
            Rename.Table("CartItem").To("Dm_Shop_CartItem");
            Delete.Table("Contract");
            Rename.Table("Dm_Account").To("Dm_IdentityAccess_Account");
            Rename.Table("Dm_Store").To("Dm_Inventory_Store");
            Rename.Table("Image").To("Dm_Inventory_ArticleFile");
            Rename.Table("MembershipRequest").To("Dm_IdentityAccess_Application");
            Rename.Table("Order").To("Dm_Shop_Order");
            Rename.Table("OrderItem").To("Dm_Shop_OrderItem");
            Rename.Table("Organization").To("Dm_IdentityAccess_Organization");
            Rename.Table("OrganizationMembership").To("Dm_IdentityAccess_Membership");
            Rename.Table("User").To("Dm_IdentityAccess_User");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}