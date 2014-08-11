namespace Phundus.Specs.Rest
{
    using System;
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

    public class OrderDetailDoc
    {
        public int OrderId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}