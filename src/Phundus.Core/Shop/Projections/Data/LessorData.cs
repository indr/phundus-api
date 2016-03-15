namespace Phundus.Shop.Projections
{
    using System;

    // TODO: Split into LessorListData and LessorDetailsData
    public class LessorData
    {
        private string _website;

        public virtual Guid LessorId { get; set; }
        public virtual LessorType Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string PostalAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool PublicRental { get; set; }

        public virtual string Website
        {
            get
            {
                if (!String.IsNullOrEmpty(_website) && !_website.StartsWith("http"))
                    return "http://" + _website;
                return _website;
            }
            set { _website = value; }
        }        
    }
}