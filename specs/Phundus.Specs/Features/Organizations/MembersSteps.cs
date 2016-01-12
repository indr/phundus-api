namespace Phundus.Specs.Features.Organizations
{
    using System.Collections.Generic;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class MembersSteps : StepsBase
    {
        private IList<Member> _members;

        public MembersSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to query all organization members")]
        public void WhenITryToQueryAllOrganizationMembers()
        {
            _members = App.GetOrganizationMembers(Ctx.Organization);
        }

        [Then(@"I should find ""(.*)"" in members")]
        public void ThenIShouldFindInMembers(string userAlias)
        {
            var user = Ctx.Users[userAlias];
            Assert.That(_members,
                Has.Some.Matches<Member>(
                    p => p.Guid == user.Guid && p.FirstName == user.FirstName && p.LastName == user.LastName));
        }
    }
}