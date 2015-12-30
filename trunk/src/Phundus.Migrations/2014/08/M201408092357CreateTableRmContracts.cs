namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201408092357)]
    public class M201408092357CreateTableRmContracts : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Rm_Contracts")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("SignedOn").AsDateTime().Nullable()
                .WithColumn("OrganizationId").AsInt32().NotNullable()
                .WithColumn("BorrowerId").AsInt32().NotNullable()
                .WithColumn("BorrowerFirstName").AsString(255).NotNullable()
                .WithColumn("BorrowerLastName").AsString(225).NotNullable()
                .WithColumn("BorrowerEmail").AsString(255).Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}