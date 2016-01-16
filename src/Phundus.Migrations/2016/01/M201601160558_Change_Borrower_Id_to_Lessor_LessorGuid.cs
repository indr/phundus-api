namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601160559)]
    public class M201601160559_Change_Borrower_Id_to_Lessee_LesseeGuid : MigrationBase
    {
        public override void Up()
        {
            const string tableName = "Dm_Shop_Order";

            Create.Column("Lessee_LesseeGuid").OnTable(tableName).AsGuid().Nullable();
            Execute.Sql(String.Format(@"UPDATE [{0}] SET [{0}].[Lessee_LesseeGuid] = [Dm_IdentityAccess_User].[Guid]
FROM [Dm_IdentityAccess_User]
WHERE [Dm_IdentityAccess_User].[Id] = [{0}].[Borrower_Id]", tableName));

            Alter.Column("Lessee_LesseeGuid").OnTable(tableName).AsGuid().NotNullable();
            Delete.Column("Borrower_Id").FromTable(tableName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}