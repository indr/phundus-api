﻿namespace Phundus.IdentityAccess.Infrastructure.Persistence.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Organizations.Model;

    public class OrganizationMap : ClassMap<Organization>
    {
        public OrganizationMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Organization");
            CompositeId(x => x.Id).KeyProperty(x => x.Id, "Guid");
            Version(x => x.Version);

            Map(x => x.EstablishedAtUtc, "CreateDate").CustomType<UtcDateTimeType>().Not.Update();
            Map(x => x.Name, "Name");
            Map(x => x.Plan, "[Plan]").CustomType<OrganizationPlan>();

            Component(x => x.ContactDetails, a =>
            {
                a.Map(x => x.Line1);
                a.Map(x => x.Line2);
                a.Map(x => x.Street);
                a.Map(x => x.Postcode);
                a.Map(x => x.City);
                a.Map(x => x.PhoneNumber, "PhoneNumber");
                a.Map(x => x.EmailAddress, "EmailAddress");
                a.Map(x => x.Website, "Website");
            });

            Map(x => x.Startpage, "Startpage");
            Map(x => x.DocTemplateFileName, "DocTemplateFileName");

            HasMany(x => x.Memberships)
                .KeyColumn("OrganizationGuid")
                .AsSet().Cascade.SaveUpdate();

            Component(x => x.Settings, a =>
                a.Map(x => x.PublicRental, "Settings_PublicRental"));
        }
    }
}