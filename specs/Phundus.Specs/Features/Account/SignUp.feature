Feature: SignUp

Scenario: Sign up sends account validation email
	When I try to sign up
	Then I should receive email "[phundus] Validierung der E-Mail-Adresse"
