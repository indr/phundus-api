Feature: Startpage

Background:
	Given an organization "Scouts" with these members
    | Alias | Role    | Email address         |
    | Greg  | Manager | greg@test.phundus.ch  |
	And I am logged in as Greg

Scenario: Update startpage updates startpage
	When I update the startpage	
	Then I should see no content
	Then I should see the updated startpage
