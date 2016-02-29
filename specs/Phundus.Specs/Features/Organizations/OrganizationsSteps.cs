namespace Phundus.Specs.Features.Organizations
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
    public class OrganizationsSteps : AppStepsBase
    {
        private Organization _organizationDetails;
        private IList<Organization> _organizations;

        public OrganizationsSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"an organization")]
        public void GivenAnOrganization()
        {
            GivenAnOrganization("");
        }

        [Given(@"an organization ""(.*)""")]
        public void GivenAnOrganization(string organizationKey)
        {
            App.LogInAsRoot();
            var organization = App.EstablishOrganization();
            Ctx.Organizations[organizationKey] = organization;
            App.DeleteSessionCookies();
        }

        [Given(@"an organization with these members")]
        public void GivenAnOrganizationWithTheseMembers(Table table)
        {
            GivenAnOrganization_WithTheseMembers("", table);
        }

        [Given(@"an organization ""(.*)"" with these members")]
        public void GivenAnOrganization_WithTheseMembers(string alias, Table table)
        {
            App.LogInAsRoot();
            Organization organization = null;
            if (Ctx.Organizations.ContainsAlias(alias))
            {
                Ctx.Organization = Ctx.Organizations[alias];
                organization = Ctx.Organization;
            }
            else
            {
                organization = App.EstablishOrganization();
                Ctx.Organization = organization;
                Ctx.Organizations[alias] = organization;
            }
            WithTheseMembers(table, organization);
        }

        [Given(@"an organization with name ""(.*)"" with these members")]
        public void GivenAnOrganizationWithNameWithTheseMembers(string name, Table table)
        {
            App.LogInAsRoot();
            Ctx.Organization = App.EstablishOrganization(name);
            WithTheseMembers(table, Ctx.Organization);
        }


        private void WithTheseMembers(Table table, Organization organization)
        {
            foreach (var each in table.Rows)
            {
                var userAlias = each["Alias"];
                if (Ctx.Users.ContainsAlias(userAlias))
                    continue;

                if (each.ContainsKey("Email address"))
                    Given(String.Format(@"a confirmed user ""{0}"" with email address ""{1}""", userAlias,
                        each["Email address"]));
                else
                    Given(@"a confirmed user """ + userAlias + @"""");
                var user = Ctx.Users[userAlias];
                App.LogIn(user);
                var applicationGuid = App.ApplyForMembership(user, organization);
                App.LogInAsRoot();
                App.ApproveMembershipApplication(organization, applicationGuid);
                if (each["Role"].ToLowerInvariant() == "manager")
                    App.ChangeMembersRole(organization.OrganizationId, user.Id, MemberRole.Manager);
            }
        }

      

        [Given(@"I established an organization")]
        public void GivenIEstablishedAnOrganization()
        {
            Ctx.Organization = App.EstablishOrganization();
        }

        [Given(@"I changed to organization contact details")]
        public void GivenIChangedToOrganizationContactDetails(Table table)
        {
            var row = table.Rows[0];
            App.ChangeOrganizationContactDetails(Ctx.Organization,
                row["Line1"], row["Line2"], row["Street"], row["Postcode"], row["City"],
                row["Phone number"], row["Email address"], row["Website"]);
        }

        [When(@"I try to establish an organization")]
        public void WhenITryToEstablishAnOrganization()
        {
            Ctx.Organization = App.EstablishOrganization(assertHttpStatus:false);
        }

        [When(@"I try to get the organization details")]
        public void WhenITryToGetTheOrganizationDetails()
        {
            _organizationDetails = App.GetOrganization(Ctx.Organization.OrganizationId);
        }

        [When(@"I try to query all organizations")]
        public void WhenITryToQueryAllOrganizations()
        {
            _organizations = App.QueryOrganizations();
        }

        [Then(@"I should find the organization in the result")]
        public void ThenIShouldFindTheOrganizationInTheResult()
        {
            Assert.That(_organizations,
                Has.Some.Matches<Organization>(p => p.OrganizationId == Ctx.Organization.OrganizationId));
        }

        [Then(@"I should see the organization details")]
        public void ThenIShouldSeeTheOrganizationDetails()
        {
            Assert.That(_organizationDetails, Is.Not.Null);
            Assert.That(_organizationDetails.OrganizationId, Is.EqualTo(Ctx.Organization.OrganizationId));
        }

        [Then(@"I should see these organization contact details")]
        public void ThenIShouldSeeTheseOrganizationContactDetails(Table table)
        {
            table.CompareToInstance(_organizationDetails.ContactDetails);
        }
    }
}