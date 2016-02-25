namespace Phundus.Common.Projections
{
    using System.Collections.Generic;

    public class QueryResult<T>
    {
        public QueryResult(int offset, int limit, long total, IList<T> result)
        {
            Offset = offset;
            Limit = limit;
            Total = total;
            Result = result;
        }

        public int Offset { get; private set; }
        public int Limit { get; private set; }
        public long Total { get; private set; }
        public IList<T> Result { get; private set; }
    }
}