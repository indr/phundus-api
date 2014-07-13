namespace Phundus.Core.Entities
{
    using System;
    using System.Text.RegularExpressions;
    using Iesi.Collections.Generic;

    public class Organization : EntityBase
    {
        ISet<OrganizationMembership> _memberships = new HashedSet<OrganizationMembership>();
        string _startpage;
        private DateTime _createDate = DateTime.Now;

        public Organization()
        {
        }

        public Organization(int id) : base(id)
        {
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
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

        public virtual string Address { get; set; }

        public virtual string Coordinate { get; set; }

        public virtual string Startpage
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_startpage))
                    return String.Format("<p>Startseite der Organisation \"{0}\".</p><p>Diese Seite kann unter \"Verwaltung\" / \"Einstellungen\" angepasst werden.", Name);
                return _startpage;
            }
            set { _startpage = value; }
        }

        public virtual string EmailAddress { get; set; }

        public virtual string Website { get; set; }

        public virtual string DocTemplateFileName { get; set; }
    }
}