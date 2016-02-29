namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602290431)]
    public class M201602290431_Create_organization_contact_details_postal_address_columns : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_IdentityAccess_Organization")
                .AddColumn("Line1").AsString().Nullable()
                .AddColumn("Line2").AsString().Nullable()
                .AddColumn("Street").AsString().Nullable()
                .AddColumn("Postcode").AsString().Nullable()
                .AddColumn("City").AsString().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}