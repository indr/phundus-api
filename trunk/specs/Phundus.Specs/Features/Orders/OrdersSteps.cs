namespace Phundus.Specs.Features.Orders
{
    using System;
    using System.Collections.Generic;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AddOrderItemSteps : AppStepsBase
    {
        public AddOrderItemSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to add the article (.+) to the order")]
        public void WhenITryToAddTheArticleFootballToTheOrder(string alias)
        {
            var article = Ctx.Articles[alias];
            App.AddOrderItem(Ctx.Order.OrderId, article.ArticleId);
        }
    }

    [Binding]
    public class OrdersSteps : AppStepsBase
    {
        private Guid _orderId;
        private IList<Order> _results;
        private Order _order;

        public OrdersSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I created a new empty order for (.+)")]
        public void GivenICreatedANewEmptyOrderForAlice(string userAlias)
        {
            var organization = Ctx.Organization;
            var lessee = Ctx.Users[userAlias];
            _orderId = App.CreateOrder(organization.OrganizationId, lessee.UserId);
            Ctx.Order = new Order {OrderId = _orderId};
        }

        [When(@"I try to create a new empty order for (.+)")]
        public void WhenICreateANewEmptyOrderFor(string userAlias)
        {
            var organization = Ctx.Organization;
            var lessee = Ctx.Users[userAlias];
            _orderId = App.CreateOrder(organization.OrganizationId, lessee.UserId);
            Ctx.Order = new Order {OrderId = _orderId};
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

        [Then(@"the order should have these items:")]
        public void ThenTheOrderShouldHaveTheseItems(Table table)
        {
            if (_order == null)
                _order = App.GetOrder(_orderId);
            table.CompareToSet(_order.Items);
        }
    }
}