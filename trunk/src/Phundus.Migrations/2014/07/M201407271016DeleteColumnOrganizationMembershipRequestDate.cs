namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201407271016)]
    public class M201407271016DeleteColumnOrganizationMembershipRequestDate : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("RequestDate").FromTable("OrganizationMembership");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}