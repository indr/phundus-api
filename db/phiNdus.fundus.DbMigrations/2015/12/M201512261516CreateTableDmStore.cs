namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512261518)]
    public class M201512261518CreateTableDmStore : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Dm_Store")
                .WithColumn("StoreId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("CreatedAtUtc").AsDateTime().NotNullable()
                .WithColumn("ModifiedAtUtc").AsDateTime().NotNullable()

                .WithColumn("Address").AsString().Nullable()
                .WithColumn("Coordinate_Latitude").AsDecimal().Nullable()
                .WithColumn("Coordinate_Longitude").AsDecimal().Nullable()
                .WithColumn("OpeningHours").AsString().Nullable()
                .WithColumn("Owner_OwnerId").AsGuid().NotNullable()
                .WithColumn("Owner_Name").AsString().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}