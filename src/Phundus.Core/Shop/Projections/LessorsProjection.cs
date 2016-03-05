namespace Phundus.Shop.Projections
{
    using System;
    using System.Text;
    using Common;
    using Common.Notifications;
    using Common.Projecting;
    using IdentityAccess.Model;
    using IdentityAccess.Model.Organizations;
    using IdentityAccess.Model.Users;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Users.Model;

    public class LessorsProjection : ProjectionBase<LessorData>,
        IConsumes<OrganizationEstablished>,
        IConsumes<OrganizationContactDetailsChanged>,
        IConsumes<PublicRentalSettingChanged>,
        IConsumes<UserSignedUp>,
        IConsumes<UserEmailAddressChanged>,
        IConsumes<UserAddressChanged>
    {
        public void Consume(OrganizationEstablished e)
        {
            Insert(x =>
            {
                x.LessorId = e.OrganizationId;
                x.Type = LessorType.Organization;
                x.Name = e.Name;
                x.Url = e.Name.ToFriendlyUrl();
                x.PostalAddress = null;
                x.PhoneNumber = null;
                x.EmailAddress = null;
                x.Website = null;
                x.PublicRental = e.PublicRental;
            });
        }

        public void Consume(OrganizationContactDetailsChanged e)
        {
            Update(e.OrganizationId, x =>
            {
                x.PostalAddress = MakeOrganizationPostalAddress(e.Line1, e.Line2, e.Street, e.Postcode, e.City);
                x.PhoneNumber = e.PhoneNumber;
                x.EmailAddress = e.EmailAddress;
                x.Website = e.Website;
            });
        }

        public void Consume(PublicRentalSettingChanged e)
        {
            Update(e.OrganizationId, x => { x.PublicRental = e.Value; });
        }

        public void Consume(UserSignedUp e)
        {
            Insert(x =>
            {
                x.LessorId = e.UserId;
                x.Type = LessorType.User;
                x.Name = e.FirstName + " " + e.LastName;
                x.PostalAddress = MakeUserPostalAddress(e.FirstName, e.LastName, e.Street, e.Postcode, e.City);
                x.PhoneNumber = e.PhoneNumber;
                x.EmailAddress = e.EmailAddress;
                x.Website = null;
                x.PublicRental = true;
            });
        }

        public void Consume(UserEmailAddressChanged e)
        {
            Update(e.UserId, x => { x.EmailAddress = e.NewEmailAddress; });
        }

        public void Consume(UserAddressChanged e)
        {
            Update(e.UserId, x =>
            {
                x.PostalAddress = MakeUserPostalAddress(e.FirstName, e.LastName, e.Street, e.Postcode, e.City);
                x.PhoneNumber = e.PhoneNumber;
            });
        }

        private string MakeOrganizationPostalAddress(string line1, string line2, string street, string postcode,
            string city)
        {
            var sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(line1))
                sb.AppendLine(line1);
            if (!String.IsNullOrWhiteSpace(line2))
                sb.AppendLine(line2);
            if (!String.IsNullOrWhiteSpace(street))
                sb.AppendLine(street);

            if (!String.IsNullOrWhiteSpace(postcode) && !String.IsNullOrWhiteSpace(city))
                sb.AppendLine(postcode + " " + city);
            else if (!String.IsNullOrWhiteSpace(postcode))
                sb.AppendLine(postcode);
            else
                sb.AppendLine(city);

            return sb.ToString();
        }

        private string MakeUserPostalAddress(string firstName, string lastName, string street, string postcode,
            string city)
        {
            var sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(firstName) && !String.IsNullOrWhiteSpace(lastName))
                sb.AppendLine(firstName + " " + lastName);
            else if (!String.IsNullOrWhiteSpace(firstName))
                sb.AppendLine(firstName);
            else
                sb.AppendLine(lastName);

            if (!String.IsNullOrWhiteSpace(street))
                sb.AppendLine(street);

            if (!String.IsNullOrWhiteSpace(postcode) && !String.IsNullOrWhiteSpace(city))
                sb.AppendLine(postcode + " " + city);
            else if (!String.IsNullOrWhiteSpace(postcode))
                sb.AppendLine(postcode);
            else
                sb.AppendLine(city);

            return sb.ToString();
        }
    }
}