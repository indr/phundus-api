namespace Phundus.Inventory.Model.Stores
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class PostalAddress : ValueObject
    {
        public PostalAddress(string line1, string line2, string street, string postcode, string city)
        {
            Line1 = line1;
            Line2 = line2;
            Street = street;
            Postcode = postcode;
            City = city;
        }

        protected PostalAddress()
        {
        }

        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string Street { get; private set; }
        public string Postcode { get; private set; }
        public string City { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Line1;
            yield return Line2;
            yield return Street;
            yield return Postcode;
            yield return City;
        }
    }
}