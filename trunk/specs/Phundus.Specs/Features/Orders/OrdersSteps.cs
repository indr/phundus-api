namespace Phundus.Specs.Features.Orders
{
    using System.Collections.Generic;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class OrdersSteps : StepsBase
    {
        private int _orderId;
        private IList<Order> _results;

        public OrdersSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I created a new empty order for (.+)")]
        public void GivenICreatedANewEmptyOrderForAlice(string userAlias)
        {
            var organization = Ctx.Organization;
            var lessee = Ctx.Users[userAlias];
            _orderId = App.CreateOrder(organization.OrganizationId, lessee.Id);
        }

        [When(@"I try to create a new empty order for (.+)")]
        public void WhenICreateANewEmptyOrderFor(string userAlias)
        {
            var organization = Ctx.Organization;
            var lessee = Ctx.Users[userAlias];
            _orderId = App.CreateOrder(organization.OrganizationId, lessee.Id);
        }

        [When(@"I try to query all orders of organization ""(.*)""")]
        public void WhenITryToQueryAllOrdersOfOrganization(string organizationAlias)
        {
            var organization = Ctx.Organizations[organizationAlias];
            _results = App.QueryOrders(organization.OrganizationId);
        }

        [Then(@"I should find the order in the results")]
        public void ThenIShouldFindTheOrderInTheResults()
        {
            Assert.That(_results, Has.Some.Matches<Order>(p => p.OrderId == _orderId));
        }

    }
}