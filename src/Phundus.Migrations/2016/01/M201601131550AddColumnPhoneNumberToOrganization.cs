namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601131550)]
    public class M201601131550AddColumnPhoneNumberToOrganization : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_IdentityAccess_Organization").AddColumn("PhoneNumber").AsString().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}