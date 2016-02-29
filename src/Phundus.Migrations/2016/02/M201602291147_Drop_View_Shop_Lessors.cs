namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602291147)]
    public class M201602291147_Drop_View_Shop_Lessors : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [View_Shop_Lessors]");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}