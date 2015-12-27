namespace Phundus.Specs.Rest
{
    using System;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Contracts")]
    [Ignore("Contracts")]
    public class when_contracts_post_is_issued : concern
    {
        private static Guid organizationId = new Guid("10000001-3781-436d-9290-54574474e8f8");

        private static IRestResponse<ContractDetailDoc> response;

        public Establish c = () => { };

        public Because of = () => { response = api.PostContract(organizationId, 10001); };

        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        public It should_return_doc_with_contract_id = () => response.Data.ContractId.ShouldBeGreaterThan(0);

        public It should_return_doc_with_created_on =
            () => response.Data.CreatedOn.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        public It should_return_doc_with_organization_id = () => response.Data.OrganizationId.ShouldEqual(organizationId);
    }

    public class ContractDetailDoc
    {
        public int ContractId { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}