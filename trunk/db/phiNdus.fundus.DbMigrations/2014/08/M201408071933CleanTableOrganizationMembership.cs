namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408071933)]
    public class M201408071933CleanTableOrganizationMembership : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("OrganizationMembership").Row(new {IsApproved = false});
            Update.Table("OrganizationMembership").Set(new {ApprovalDate = DateTime.UtcNow}).Where(new {ApprovalDate = (DateTime?)null});
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}