Feature: ContactDetails

Background:
	Given an organization "Scouts" with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg

Scenario: Change contact details
	Given I changed to organization contact details
	| Post address | Phone number  | Email address          | Website               |
	| Street 123   | 012 345 67 89 | scouts@test.phundus.ch | http://www.scouts.com |
	When I try to get the organization details
	Then I should see these organization contact details
	| Post address | Phone number  | Email address          | Website               |
	| Street 123   | 012 345 67 89 | scouts@test.phundus.ch | http://www.scouts.com |

