Feature: Startpage

Background:
	Given an organization "Scouts" with these members
    | Alias | Role    | Email address         |
    | Greg  | Manager | greg@test.phundus.ch  |
	And I am logged in as Greg

Scenario: Update startpage returns no content
	When I try to update the startpage
	Then I should see accepted
	
Scenario: Update startpage updates startpage
	Given I updated the startpage
	When I try to get the startpage
	Then I should see the updated startpage
