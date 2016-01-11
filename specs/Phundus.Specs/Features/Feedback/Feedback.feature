Feature: Feedback

@isSmoker
Scenario: Send as anon	
	When submit feedback as anon with comment:
		"""
		Greetings from feedback feature scenario "Send as anon"!
		
		Server URL: {AppSettings.ServerUrl}
		Version: {Assembly.Version}
		"""
	Then anon should receive email "Vielen Dank fürs Feedback" with text body:
		"""
		Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

		Vielen Dank And freundliche Grüsse

		Das phundus-Team

		--
		This is an automatically generated message from phundus.
		-
		If you think it was sent incorrectly contact the administrators at lukas.mueller@phundus.ch or reto.inderbitzin@phundus.ch.
		"""
	And "admin@test.phundus.ch" should receive email "[phundus] Feedback"

