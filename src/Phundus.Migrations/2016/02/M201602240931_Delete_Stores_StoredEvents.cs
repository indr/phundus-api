namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(2016022409031)]
    public class M201602240931_Delete_Stores_StoredEvents : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [StoredEvents]  WHERE [TypeName] LIKE '%Stores.Model%' AND [Serialization] = 0");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}