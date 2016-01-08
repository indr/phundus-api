namespace Phundus.Specs.Services.Entities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    public class FakeGenerator
    {
        private int _nextId;
        private IList<FakeNameGeneratorRow> _records;        

        public FakeGenerator()
        {
            ReadRecordsFromResource();

            _nextId = new Random().Next(0, _records.Count - 1) - 1;            
        }

        private void ReadRecordsFromResource()
        {
            var byteArray = Encoding.UTF8.GetBytes(Resources.FakeNameGenerator_com);
            var stream = new MemoryStream(byteArray);
            _records = CsvImporter.GetRecords<FakeNameGeneratorRow>(stream);
        }

        public User NextUser()
        {
            var guid = Guid.NewGuid();
            var record = GetNextRecord();
            return new User
            {
                FirstName = record.GivenName,
                Guid = guid,
                LastName = record.Surname,
                EmailAddress = record.GivenName + record.Surname
                               + "-" + guid.ToString("D").Substring(0, 6) + "@test.phundus.ch",
                City = record.City,
                Street = record.StreetAddress,
                MobilePhone = record.TelephoneNumber,
                Postcode = record.ZipCode
            };
        }

        public Organization NextOrganization()
        {
            var record = GetNextRecord();
            return new Organization
            {
                Guid = new Guid(record.Guid),
                Name = record.Company
            };
        }

        private FakeNameGeneratorRow GetNextRecord()
        {
            var result = _records[_nextId++];

            if (_nextId >= _records.Count)
                _nextId = 0;

            return result;
        }
    }
}