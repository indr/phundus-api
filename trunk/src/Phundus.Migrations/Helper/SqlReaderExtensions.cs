namespace Phundus.Migrations
{
    using System.Data;

    public static class DataReaderExtensions
    {
        public static string GetStringOrNull(this IDataReader reader, int i)
        {
            return reader.IsDBNull(i) ? null : reader.GetString(i);
        }
    }
}