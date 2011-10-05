using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public int ArticleId { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
