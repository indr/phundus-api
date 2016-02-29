namespace Phundus.Migrations
{
    using System;
    using System.Data.SqlClient;
    using FluentMigrator;

    [Migration(201602290927)]
    public class M201602290927_Remove_organization_Reserved : DataMigrationBase
    {
        protected override void Migrate()
        {
            var reservedId = new Guid("1E2311AD-2340-4AB1-BE0E-54DA9658FBD7");            
            var command = CreateCommand(@"DELETE FROM [Dm_IdentityAccess_Organization] WHERE [Guid] = @OrganizationId");
            command.Parameters.Add(new SqlParameter("@OrganizationId", reservedId));
            command.ExecuteNonQuery();
        }
    }
}