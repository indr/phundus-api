namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512300956)]
    public class M201512300956CreateOrganizationsStore : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_Store").InSchema(SchemaName).AlterColumn("Coordinate_Latitude").AsFloat().Nullable();
            Alter.Table("Dm_Store").InSchema(SchemaName).AlterColumn("Coordinate_Longitude").AsFloat().Nullable();
            Create.UniqueConstraint().OnTable("Dm_Store").Column("Owner_OwnerId");

            Execute.Sql(@"INSERT INTO [Dm_Store] ([StoreId], [Version], [CreatedAtUtc], [ModifiedAtUtc], [Address], [Owner_OwnerId], [Owner_Name],
 [Coordinate_Latitude], [Coordinate_Longitude])
SELECT NEWID(), 1, o.[CreateDate], o.[CreateDate], o.[Address], o.[Guid], o.[Name],
SUBSTRING(o.[Coordinate], 0, CHARINDEX(',', o.[Coordinate])),
SUBSTRING(o.[Coordinate], CHARINDEX(',', o.[Coordinate])+1, LEN(o.[Coordinate]) - CHARINDEX(',', o.[Coordinate]))
FROM [Organization] o");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}