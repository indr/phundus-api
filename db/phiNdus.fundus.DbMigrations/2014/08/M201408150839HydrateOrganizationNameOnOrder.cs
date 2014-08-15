namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408150839)]
    public class M201408150839HydrateOrganizationNameOnOrder : HydrationBase
    {
        protected override void Hydrate()
        {
            const string fmtUpdate = "update [order] set [organization_name] = '{1}' where [organizationid] = {0}";
            using (var command = CreateCommand("select id, name from organization"))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                    Commands.Add(String.Format(fmtUpdate, reader["Id"], reader["Name"].ToString().Replace("'", "''")));
            }

            ExecuteCommands();
        }
    }
}