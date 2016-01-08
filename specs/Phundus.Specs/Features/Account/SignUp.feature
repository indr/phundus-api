Feature: SignUp

Scenario: Sign up sends account validation email
	When sign up
	Then user should receive email "[phundus] Validierung der E-Mail-Adresse"
