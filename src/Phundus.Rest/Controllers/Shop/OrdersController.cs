namespace Phundus.Rest.Controllers.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Castle.Transactions;
    using Core.Shop.Queries;

    public class OrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId)
        {
            var result = OrderQueries.FindByOrganizationId(organizationId, CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, ToDocs(result));
        }

        private static ICollection<OrderDoc> ToDocs(IEnumerable<OrderDto> dtos)
        {
            return dtos.Select(each => new OrderDoc
            {
                OrderId = each.Id,
                Version = each.Version,
                OrganizationId = each.OrganizationId,
                CreatedOn = each.CreateDate,
                ReserverId = each.ReserverId,
                ReserverFirstName = each.ReserverName.Split(' ').FirstOrDefault(),
                ReserverLastName = each.ReserverName.Split(' ').LastOrDefault(),
                Status = each.Status.ToString()
            }).ToList();
        }
    }

    public class OrderDoc
    {
        public int OrderId { get; set; }
        public int Version { get; set; }
        public int OrganizationId { get; set; }

        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }

        public int ReserverId { get; set; }
        public string ReserverFirstName { get; set; }
        public string ReserverLastName { get; set; }
    }
}