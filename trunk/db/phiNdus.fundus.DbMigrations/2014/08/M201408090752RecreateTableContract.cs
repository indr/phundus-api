namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408090752)]
    public class M201408090752RecreateTableContract : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("ContractItem");
            Delete.Table("Contract");
            
            Create.Table("Contract")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()

                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("SignedOn").AsDateTime().Nullable()
                .WithColumn("OrganizationId").AsInt32().NotNullable()

                .WithColumn("Borrower_Id").AsInt32().NotNullable()
                .WithColumn("Borrower_FirstName").AsString(255).NotNullable()
                .WithColumn("Borrower_LastName").AsString(225).NotNullable()
                .WithColumn("Borrower_Email").AsString(255).Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}