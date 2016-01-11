namespace Phundus.Specs.Services.Entities
{
    using CsvHelper.Configuration;

    public class ArticleNameRow
    {
        [CsvField(Index=0)]
        public string Name { get; set; }
    }

    public class FakeNameGeneratorRow
    {
        [CsvField(Name = "GUID")]
        public string Guid { get; set; }

        [CsvField(Name = "Password")]
        public string Password { get; set; }

        [CsvField(Name = "Company")]
        public string Company { get; set; }

        [CsvField(Name = "GivenName")]
        public string GivenName { get; set; }

        [CsvField(Name = "Surname")]
        public string Surname { get; set; }

        [CsvField(Name = "StreetAddress")]
        public string StreetAddress { get; set; }

        [CsvField(Name = "ZipCode")]
        public string ZipCode { get; set; }

        [CsvField(Name = "City")]
        public string City { get; set; }

        [CsvField(Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [CsvField(Name = "TelephoneNumber")]
        public string TelephoneNumber { get; set; }
    }
}