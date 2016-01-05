namespace Phundus.Specs.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("/api/orders")]
    public class when_orders_post_is_issued : concern
    {
        private static Guid organizationId = new Guid("10CE0EC8-3781-436d-9290-54574474e8f8");

        public static IRestResponse<OrderDetailDoc> response;

        public Because of = () => { response = api.PostOrder(organizationId, 10003); };

        public It should_return_doc_with_created_on =
            () => response.Data.CreatedAtUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        public It should_return_doc_with_order_id = () => response.Data.OrderId.ShouldBeGreaterThan(0);

        public It should_return_doc_with_lessor_id = () => response.Data.LessorId.ShouldEqual(organizationId);

        public It should_return_doc_with_status_pending = () => response.Data.Status.ShouldEqual("Pending");
        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public abstract class organizations_orders_patch_concern : concern
    {
        protected static Guid organizationId = new Guid("10CE0EC8-3781-436d-9290-54574474e8f8");
        protected static int orderId;
        protected static IRestResponse<OrderDetailDoc> response;

        public Establish ctx = () => { orderId = api.PostOrder(organizationId, 10003).Data.OrderId; };
    }

    [Subject("/api/orders")]
    public class when_orders_patch_to_reject_order_is_issued : organizations_orders_patch_concern
    {
        public Because of = () => response = api.PatchOrder(organizationId, orderId, "Rejected");

        public It should_return_doc_with_status_rejected = () => response.Data.Status.ShouldEqual("Rejected");
        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Subject("/api/orders")]
    public class when_orders_patch_to_approve_order_is_issued : organizations_orders_patch_concern
    {
        public Because of = () => response = api.PatchOrder(organizationId, orderId, "Approved");

        public It should_return_doc_with_status_rejected = () => response.Data.Status.ShouldEqual("Approved");
        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Subject("/api/orders")]
    public class when_orders_patch_to_close_order_is_issued : organizations_orders_patch_concern
    {
        public Because of = () => response = api.PatchOrder(organizationId, orderId, "Closed");

        public It should_return_doc_with_status_rejected = () => response.Data.Status.ShouldEqual("Closed");
        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Subject("OrdersItems")]
    public class when_orders_items_post_is_issued : concern
    {
        private static Guid organizationId = new Guid("10CE0EC8-3781-436d-9290-54574474e8f8");
        private static int orderId;
        private static IRestResponse<OrderItemDoc> response;

        public Establish c = () => { orderId = api.PostOrder(organizationId, 10003).Data.OrderId; };

        public Because of = () => { response = api.PostOrderItem(organizationId, orderId); };

        public It should_return_doc_order_item_id = () => response.Data.OrderItemId.ShouldNotEqual(Guid.Empty);
        public It should_return_status_created = () => response.StatusCode.ShouldEqual(HttpStatusCode.Created);
    }

    [Subject("OrderItems")]
    public class when_orders_items_patch_is_issued : concern
    {
        private static Guid organizationId = new Guid("10CE0EC8-3781-436d-9290-54574474e8f8");
        private static int orderId;
        private static Guid orderItemId;
        private static IRestResponse response;

        public Establish c = () =>
        {
            orderId = api.PostOrder(organizationId, 10003).Data.OrderId;
            orderItemId = api.PostOrderItem(organizationId, orderId).Data.OrderItemId;
        };

        public Because of =
            () =>
            {
                response = api.UpdateOrderItem(organizationId, orderId, orderItemId, DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow.AddDays(2), 2);
            };

        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Subject("OrdersItems")]
    public class when_orders_items_delete_is_issued : concern
    {
        private static Guid organizationId = new Guid("10CE0EC8-3781-436d-9290-54574474e8f8");
        private static int orderId;
        private static Guid orderItemId;
        private static IRestResponse response;

        public Establish c = () =>
        {
            orderId = api.PostOrder(organizationId, 10003).Data.OrderId;
            orderItemId = api.PostOrderItem(organizationId, orderId).Data.OrderItemId;
        };

        public Because of = () => { response = api.DeleteOrderItem(organizationId, orderId, orderItemId); };

        public It should_return_status_no_content = () => response.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    }

    public class OrderDetailDoc
    {
        private List<OrderItemDoc> _items = new List<OrderItemDoc>();

        public int OrderId { get; set; }
        public Guid LessorId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string Status { get; set; }

        public List<OrderItemDoc> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }

    public class OrderItemDoc
    {
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }
    }
}