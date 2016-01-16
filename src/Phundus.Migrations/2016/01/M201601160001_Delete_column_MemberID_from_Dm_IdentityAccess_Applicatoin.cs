namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601160001)]
    public class M201601160001_Delete_column_MemberID_from_Dm_IdentityAccess_Applicatoin : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("MemberId").FromTable("Dm_IdentityAccess_Application").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}