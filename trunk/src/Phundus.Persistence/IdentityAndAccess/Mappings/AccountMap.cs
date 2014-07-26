namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccessCtx.Model;
    using FluentNHibernate;
    using FluentNHibernate.Mapping;

    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Table("Membership");
            Id(x => x.Id).GeneratedBy.Foreign("User");
            Version(x => x.Version);

            HasOne(x => x.User).Constrained();

            Map(x => x.SessionKey).Unique().Length(24);
            Map(x => x.Password).Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore);
            Map(x => x.Salt);
            Map(x => x.Email).UniqueKey("MembershipEmail");
            Map(x => x.RequestedEmail);
            Map(x => x.IsApproved);
            Map(x => x.IsLockedOut);
            Map(x => x.CreateDate).ReadOnly();
            Map(x => x.LastLogOnDate);
            Map(x => x.Comment);
            Map(x => x.ValidationKey);
        }
    }
}