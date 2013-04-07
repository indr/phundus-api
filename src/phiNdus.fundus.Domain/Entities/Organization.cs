namespace phiNdus.fundus.Domain.Entities
{
    using System.Text.RegularExpressions;
    using Iesi.Collections.Generic;

    public class Organization : EntityBase
    {
        ISet<OrganizationMembership> _memberships = new HashedSet<OrganizationMembership>();

        public Organization()
        {
        }

        public Organization(int id) : base(id)
        {
        }

        public virtual string Name { get; set; }

        public virtual string Url
        {
            get
            {
                // http://stackoverflow.com/questions/37809/how-do-i-generate-a-friendly-url-in-c
                // http://stackoverflow.com/questions/2161684/transform-title-into-dashed-url-friendly-string

                var url = Name.ToLowerInvariant();
                url = Regex.Replace(url, " ", "-");
                return Regex.Replace(url, @"[^A-Za-zÄÖÜäöü0-9\-\._~]+", "");
            }
        }

        public virtual ISet<OrganizationMembership> Memberships
        {
            get { return _memberships; }
            set { _memberships = value; }
        }
    }
}