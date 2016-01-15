﻿namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using IdentityAccess.Organizations.Model;

    public class MembershipApplicationMap : ClassMap<MembershipApplication>
    {
        public MembershipApplicationMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Application");
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.OrganizationId, "OrganizationGuid");
            //Map(x => x.UserId, "MemberId").ReadOnly();
            Component(x => x.UserGuid, a => a.Map(x => x.Id, "UserGuid"));
            //References(x => x.User, "MemberId").Cascade.None();

            Map(x => x.RequestDate);
            Map(x => x.ApprovalDate);
            Map(x => x.RejectDate);
        }
    }
}