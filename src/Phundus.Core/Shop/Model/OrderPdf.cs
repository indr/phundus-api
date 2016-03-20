namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common.Domain.Model;

    public class OrderPdf : ValueObject
    {
        private readonly Stream _stream;

        public OrderPdf(OrderId orderId, OrderShortId orderShortId, int version, Stream stream)
        {
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (stream == null) throw new ArgumentNullException("stream");
            OrderId = orderId;
            OrderShortId = orderShortId;
            Version = version;
            _stream = stream;
        }

        public OrderId OrderId { get; private set; }
        public int Version { get; private set; }
        public OrderShortId OrderShortId { get; private set; }

        public Stream GetStreamCopy()
        {
            var result = new MemoryStream();
            _stream.Seek(0, SeekOrigin.Begin);
            _stream.CopyTo(result);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OrderId;
            yield return Version;
        }
    }
}