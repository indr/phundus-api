namespace Phundus.Specs.Services.Entities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CsvHelper.Configuration;

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

    public class CsvImporter
    {
        public static IList<T> GetRecords<T>(string fileName) where T : class
        {
            return GetRecords<T>(new StreamReader(fileName));
        }

        public static IList<T> GetRecords<T>(StreamReader streamReader) where T : class
        {
            var configuration = new CsvConfiguration
            {
                Delimiter = ",",
                Encoding = Encoding.UTF8
            };
            var csv = new CsvHelper.CsvReader(streamReader, configuration);
            return csv.GetRecords<T>().ToList();
        }

        public static IList<T> GetRecords<T>(Stream stream) where T : class
        {
            return GetRecords<T>(new StreamReader(stream));
        }
    }
}