namespace Phundus.Specs.Services.Entities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class RecordsReader
    {
        public static IList<T> ReadFromResource<T>(string resourceName) where T : class
        {
            var byteArray = Encoding.UTF8.GetBytes(resourceName);
            var stream = new MemoryStream(byteArray);
            return CsvReader.GetRecords<T>(stream);
        }
    }
}