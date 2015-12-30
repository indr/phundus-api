namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201408121620)]
    public class M201408121620CompleteBorrowerOnContract : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Contract")
                .AddColumn("Borrower_Street").AsString(255).Nullable()
                .AddColumn("Borrower_Postcode").AsString(255).Nullable()
                .AddColumn("Borrower_City").AsString(255).Nullable()
                .AddColumn("Borrower_MobilePhoneNumber").AsString(255).Nullable()
                .AddColumn("Borrower_MemberNumber").AsString(255).Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}