namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201408150837)]
    public class M201408150837CreateColumnOrganizationNameOnOrder : MigrationBase
    {
        public override void Up()
        {
            Create.Column("Organization_Name").OnTable("Order").AsString(255).Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}