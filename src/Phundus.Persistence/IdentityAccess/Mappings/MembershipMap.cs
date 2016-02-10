﻿namespace Phundus.Persistence.IdentityAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.IdentityAccess.Organizations.Model;

    public class MembershipMap : ClassMap<Membership>
    {
        public MembershipMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Membership");
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            //Map(x => x.UserId, "UserId");
            Component(x => x.UserId, a => a.Map(x => x.Id, "UserGuid"));

            References(x => x.Organization, "OrganizationGuid").Cascade.None();

            //Map(x => x.OrganizationGuid, "OrganizationGuid");

            Map(x => x.Role).CustomType<Role>();
            Map(x => x.ApprovalDate, "ApprovalDate");
            Map(x => x.IsLocked);
        }
    }
}