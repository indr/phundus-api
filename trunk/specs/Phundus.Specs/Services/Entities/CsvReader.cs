namespace Phundus.Specs.Services.Entities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CsvHelper.Configuration;

    public class CsvReader
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