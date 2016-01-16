namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601160740)]
    public class M201601160740_Add_column_CustomMemberNumber_to_Dm_IdentityAccess_Application : MigrationBase
    {
        public override void Up()
        {
            Create.Column("CustomMemberNumber").OnTable("Dm_IdentityAccess_Application").AsString().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}