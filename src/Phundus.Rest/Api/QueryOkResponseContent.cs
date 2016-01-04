namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryOkResponseContent<T>
    {
        [JsonProperty("results")]
        public IList<T> Results { get; set; }
    }
}