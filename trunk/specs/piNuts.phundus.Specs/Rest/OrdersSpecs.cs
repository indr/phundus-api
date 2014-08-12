namespace Phundus.Specs.Rest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Orders")]
    public class when_orders_post_is_issued : concern
    {
        public static IRestResponse<OrderDetailDoc> response;

        public Because of = () => { response = api.PostOrder(1001, "user-1@test.phundus.ch"); };

        public It should_return_doc_with_contract_id = () => response.Data.OrderId.ShouldBeGreaterThan(0);

        // TODO: Should be UtcNow
        public It should_return_doc_with_created_on =
            () => response.Data.CreatedOn.ShouldBeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        public It should_return_doc_with_organization_id = () => response.Data.OrganizationId.ShouldEqual(1001);
        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Subject("OrdersItems")]
    public class when_orders_items_post_is_issued : concern
    {
        private const int organizationId = 1001;
        private static int orderId;
        private static OrderItemDoc orderItemDoc;
        private static IRestResponse<OrderItemDoc> response;
        private static OrderDetailDoc orderDoc;

        public Establish c = () =>
        {
            orderId = api.PostOrder(1001, "user-1@test.phundus.ch").Data.OrderId;
        };

        public Because of = () =>
        {
            response = api.PostOrderItem(organizationId, orderId);
        };

        public It should_return_status_created = () => response.StatusCode.ShouldEqual(HttpStatusCode.Created);

        public It should_return_doc_order_item_id = () => response.Data.OrderItemId.ShouldNotEqual(Guid.Empty);
    }

    [Subject("OrderItems")]
    public class when_orders_items_patch_is_issued : concern
    {
        private const int organizationId = 1001;
        private static int orderId;
        private static Guid orderItemId;
        private static IRestResponse response;

        public Establish c = () =>
        {
            orderId = api.PostOrder(organizationId, "user-1@test.phundus.ch").Data.OrderId;
            orderItemId = api.PostOrderItem(organizationId, orderId).Data.OrderItemId;
        };

        public Because of = () =>
        {
            response = api.UpdateOrderItem(organizationId, orderId, orderItemId, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 2);
        };

        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);  
    }

    [Subject("OrdersItems")]
    public class when_orders_items_delete_is_issued : concern
    {
        private const int organizationId = 1001;
        private static int orderId;
        private static Guid orderItemId;
        private static IRestResponse response;

        public Establish c = () =>
        {
            orderId = api.PostOrder(organizationId, "user-1@test.phundus.ch").Data.OrderId;
            orderItemId = api.PostOrderItem(organizationId, orderId).Data.OrderItemId;
        };

        public Because of = () =>
        {
            response = api.DeleteOrderItem(organizationId, orderId, orderItemId);
        };

        public It should_return_status_no_content = () => response.StatusCode.ShouldEqual(HttpStatusCode.NoContent);        
    }

    public class OrderDetailDoc
    {
        private List<OrderItemDoc> _items = new List<OrderItemDoc>();

        public int OrderId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }

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