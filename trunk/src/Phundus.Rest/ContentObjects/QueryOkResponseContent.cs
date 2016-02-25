namespace Phundus.Rest.ContentObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Api.Shop;
    using Common.Projections;
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

        private QueryOkResponseContent(int offset, int limit, long total, IList<T> results)
        {
            Offset = offset;
            Limit = limit;
            Total = total;
            Results = results;
        }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("results")]
        public IList<T> Results { get; set; }

        public static QueryOkResponseContent<ShopQueryItem> Build<TSource>(QueryResult<TSource> results,
            Func<TSource, ShopQueryItem> selector)
        {
            return new QueryOkResponseContent<ShopQueryItem>(results.Offset, results.Limit, results.Total,
                results.Result.Select(selector).ToList());
        }
    }
}