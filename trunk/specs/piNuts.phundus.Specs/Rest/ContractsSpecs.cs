namespace Phundus.Specs.Rest
{
    using System;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Contracts")]
    public class when_post_is_issued : concern
    {
        private static RestRequest request;
        private static IRestResponse<ContractDetailDoc> response;

        public Establish c = () => { };

        public Because of = () => { response = api.PostContract(1001, 10001); };

        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        public It should_return_doc_with_contract_id = () => response.Data.ContractId.ShouldBeGreaterThan(0);

        public It should_return_doc_with_created_on =
            () => response.Data.CreatedOn.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        public It should_return_doc_with_organization_id = () => response.Data.OrganizationId.ShouldEqual(1001);
    }

    public class ContractDetailDoc
    {
        public int ContractId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}