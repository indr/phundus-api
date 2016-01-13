namespace Phundus.Specs.Services
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using RestSharp.Serializers;

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";
        }
    }

    public class CustomJsonSerializer : ISerializer
    {
        public CustomJsonSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings();
            //settings.DateParseHandling = DateParseHandling.DateTimeOffset;
            settings.Converters.Add(new CustomDateTimeConverter());
            return JsonConvert.SerializeObject(obj, settings);
        }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public string ContentType { get; set; }
    }
}