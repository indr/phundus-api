namespace Phundus.Migrations
{
    using System.Collections.Generic;
    using FluentMigrator;

    [Migration(201408071444)]
    public class M201408071444HydrateRmRelationships : HydrationBase
    {
        protected override void Hydrate()
        {
            var inserts = new List<string>();
            const string insertFmt =
                "insert into [Rm_Relationships] (UserId, OrganizationId, Status, [Timestamp]) values ({0}, {1}, 1, '{2}')";

            using (var cmd = CreateCommand("select UserId, OrganizationId, ApprovalDate from [OrganizationMembership] where ApprovalDate is not null"))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    inserts.Add(string.Format(insertFmt, reader.GetInt32(0), reader.GetInt32(1),
                        reader.GetDateTime(2).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss")));
                }

            ExecuteCommands(inserts);
        }
    }
}