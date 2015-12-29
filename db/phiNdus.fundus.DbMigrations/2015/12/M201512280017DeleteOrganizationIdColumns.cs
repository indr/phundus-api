namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512280017)]
    public class M201512280017DeleteOrganizationIdColumns : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("Fk_ArticleToOrganization").OnTable("Article").InSchema(SchemaName);
            Delete.Column("OrganizationId").FromTable("Article").InSchema(SchemaName);
            
            Delete.Column("OrganizationId").FromTable("MembershipRequest").InSchema(SchemaName);
            
            Delete.ForeignKey("FkOrganizationMembershipToOrganization").OnTable("OrganizationMembership").InSchema(SchemaName);
            Delete.UniqueConstraint("IX_UserId_OrganizationId").FromTable("OrganizationMembership").InSchema(SchemaName);
            Delete.Column("OrganizationId").FromTable("OrganizationMembership").InSchema(SchemaName);
            Create.UniqueConstraint().OnTable("OrganizationMembership").WithSchema(SchemaName).Columns(new[] { "OrganizationGuid", "UserId" });

            Delete.PrimaryKey("PK_Organization").FromTable("Organization").InSchema(SchemaName);
            Delete.Column("Id").FromTable("Organization").InSchema(SchemaName);
            Create.PrimaryKey().OnTable("Organization").Column("Guid");

            Delete.PrimaryKey("PK_Rm_Relationships").FromTable("Rm_Relationships").InSchema(SchemaName);
            Delete.Column("OrganizationId").FromTable("Rm_Relationships").InSchema(SchemaName);
            Create.PrimaryKey().OnTable("Rm_Relationships").Columns(new[] {"OrganizationGuid", "UserId"});
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}