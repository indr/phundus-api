namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601100554)]
    public class M201601100554RenameMembershipToAccount : MigrationBase
    {
        public override void Up()
        {            
            Rename.Table("Membership").To("Dm_Account");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}