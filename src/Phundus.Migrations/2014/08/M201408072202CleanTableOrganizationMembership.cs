namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201408072202)]
    public class M201408072202CleanTableOrganizationMembership : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("IsApproved").FromTable("OrganizationMembership");
            
            Update.Table("OrganizationMembership").Set(new{IsLocked = false}).Where(new {Role = 2});
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}