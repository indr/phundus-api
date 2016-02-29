namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602281813)]
    public class M201602281813_Reset_UserAddress_projection : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_IdentityAccess_UserAddress");
            DeleteTracker("UserAddressProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}