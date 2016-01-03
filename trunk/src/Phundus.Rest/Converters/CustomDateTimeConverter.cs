namespace Phundus.Rest.Converters
{
    using Newtonsoft.Json.Converters;

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";
        }
    }
}