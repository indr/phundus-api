namespace Phundus.Rest.ContentObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class QueryOkResponseContent<T>
    {
        public QueryOkResponseContent()
        {
            Results = new List<T>();
        }

        public QueryOkResponseContent(IEnumerable<T> results)
        {
            Results = results.ToList();
        }

        [JsonProperty("results")]
        public IList<T> Results { get; set; }
    }
}