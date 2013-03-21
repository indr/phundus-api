using System;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class ArticleBuilder : BuilderBase<Article>
    {
        private Organization _organization;

        public override Article Build()
        {
            var result = new Article();
            result.Organization = _organization;
            Persist(result);
            return result;
        }

        public ArticleBuilder Organization(Organization organization)
        {
            _organization = organization;
            return this;
        }
    }

    public class OrganizationBuilder : BuilderBase<Organization>
    {
        public override Organization Build()
        {
            var result = new Organization();
            result.Name = "Pfadi Lego";
            Persist(result);
            return result;
        }
    }
}