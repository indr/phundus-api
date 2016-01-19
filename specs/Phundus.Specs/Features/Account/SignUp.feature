Feature: SignUp

Scenario: Sign up sends account validation email
	When I try to sign up
	Then I should receive email "[phundus] Validierung der E-Mail-Adresse"

Scenario: Sign up with organization membership
	Given an organization
	When I try to sign up with membership
	Then I should receive email "[phundus] Validierung der E-Mail-Adresse"
