namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601100555)]
    public class M201601100555AddUqEmailToAccount : MigrationBase
    {
        public override void Up()
        {
            Create.UniqueConstraint().OnTable("Dm_Account").Column("Email");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}