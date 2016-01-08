Feature: Reset password
	When ein Benutzer sein Passwort nicht mehr weiss, Then kann er ein neues Passwort beantragen.
	Dazu wird dem Benutzer ein E-Mail gesendet, welches einen Link enthält. Wird dieser Link
	aufgerufen, so wird das neue Passwort aktiviert.

Background: 
	Given a confirmed user
	And logged in as user

Scenario: sends email "Neues Passwort"
	When reset password
	Then user should receive email "[phundus] Neues Passwort"
