namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;
    
    [Migration(201408121455)]
    public class M201408121455ReplaceUserWithBorrowerOnTableOrder : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Order")
                .AddColumn("Borrower_Id").AsInt32().Nullable()
                .AddColumn("Borrower_FirstName").AsString(255).Nullable()
                .AddColumn("Borrower_LastName").AsString(225).Nullable()
                .AddColumn("Borrower_EmailAddress").AsString(255).Nullable()
                .AddColumn("Borrower_Street").AsString(255).Nullable()
                .AddColumn("Borrower_Postcode").AsString(255).Nullable()
                .AddColumn("Borrower_City").AsString(255).Nullable()
                .AddColumn("Borrower_MobilePhoneNumber").AsString(255).Nullable()
                .AddColumn("Borrower_MemberNumber").AsString(255).Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}