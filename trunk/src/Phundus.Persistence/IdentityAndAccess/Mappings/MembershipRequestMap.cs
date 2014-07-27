﻿namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Core.IdentityAndAccess.Organizations.Model;
    using FluentNHibernate.Mapping;

    public class MembershipRequestMap : ClassMap<MembershipRequest>
    {
        public MembershipRequestMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.OrganizationId);
            Map(x => x.UserId, "MemberId").ReadOnly();
            References(x => x.User, "MemberId").Cascade.None();

            Map(x => x.RequestDate);
            Map(x => x.ApprovalDate);
            Map(x => x.RejectDate);
        }
    }
}