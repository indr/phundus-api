namespace Phundus.Migrations
{
    using System.Data;
    using FluentMigrator;

    [Migration(201204211645)]
    public class M0003_CreateTableMembership : MigrationBase
    {
        private const string TableName = "Membership";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // Foreign User-Identity 
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("SessionKey").AsString(24).Nullable()
                .WithColumn("Password").AsString(255).Nullable()
                .WithColumn("Salt").AsString(5).Nullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("IsApproved").AsBoolean().NotNullable()
                .WithColumn("IsLockedOut").AsBoolean().NotNullable()
                .WithColumn("CreateDate").AsDateTime().NotNullable()
                .WithColumn("LastLogOnDate").AsDateTime().Nullable()
                .WithColumn("LastPasswordChangeDate").AsDateTime().Nullable()
                .WithColumn("LastLockoutDate").AsDateTime().Nullable()
                .WithColumn("Comment").AsString(255).Nullable()
                .WithColumn("ValidationKey").AsString(24).Nullable();

            Create.Index().OnTable(TableName).InSchema(SchemaName)
                .OnColumn("Email").Ascending().WithOptions().Unique();

            Create.ForeignKey("FkMembershipToUser")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("Id")
                .ToTable("User").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.ForeignKey("FkMembershipToUser").OnTable(TableName).InSchema(SchemaName);

            Delete.Index().OnTable(TableName).InSchema(SchemaName).OnColumn("Email");

            Delete.Table(TableName);
        }
    }
}