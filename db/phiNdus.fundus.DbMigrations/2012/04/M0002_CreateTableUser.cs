using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211638)]
    public class M0002_CreateTableUser : MigrationBase
    {
        private const string TableName = "User";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("RoleId").AsInt32().Nullable()
                .WithColumn("FirstName").AsString(255).Nullable()
                .WithColumn("LastName").AsString(255).Nullable();

            //Execute.Sql(@"dbcc checkident('User', reseed, 1000)");
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}