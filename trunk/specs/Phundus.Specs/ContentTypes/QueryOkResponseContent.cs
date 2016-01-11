namespace Phundus.Specs.ContentTypes
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryOkResponseContent<T>
    {
        public QueryOkResponseContent()
        {
            Results = new List<T>();
        }

        [JsonProperty("results")]
        public List<T> Results { get; set; }
    }
}