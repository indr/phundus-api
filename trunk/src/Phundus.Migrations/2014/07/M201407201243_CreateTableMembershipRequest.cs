namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201407201243)]
    public class M201407201243_CreateTableMembershipRequest : MigrationBase
    {
        public override void Up()
        {
            Create.Table("MembershipRequest").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("OrganizationId").AsInt32().NotNullable()
                .WithColumn("MemberId").AsInt32().NotNullable()
                .WithColumn("RequestDate").AsDateTime().NotNullable()
                .WithColumn("RejectDate").AsDateTime().Nullable()
                .WithColumn("ApprovalDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("MembershipRequest").InSchema(SchemaName);
        }
    }
}