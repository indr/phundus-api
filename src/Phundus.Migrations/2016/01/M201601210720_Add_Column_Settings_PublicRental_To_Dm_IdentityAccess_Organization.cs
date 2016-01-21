namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601210720)]
    public class M201601210720_Add_Column_Settings_PublicRental_To_Dm_IdentityAccess_Organization : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_IdentityAccess_Organization")
                .AddColumn("Settings_PublicRental").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}