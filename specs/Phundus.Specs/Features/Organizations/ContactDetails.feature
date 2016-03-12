Feature: ContactDetails

Background:
	Given an organization "Scouts" with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg

Scenario: Change contact details
	When I change the organizations contact details
	| Line1 | Line2 | Street     | Postcode | City | Phone number  | Email address          | Website               |
	| Line1 | Line2 | Street 123 | 1234     | City | 012 345 67 89 | scouts@test.phundus.ch | http://www.scouts.com |	
	Then I should see these organization contact details
	| Line1 | Line2 | Street     | Postcode | City | Phone number  | Email address          | Website               |
	| Line1 | Line2 | Street 123 | 1234     | City | 012 345 67 89 | scouts@test.phundus.ch | http://www.scouts.com |

