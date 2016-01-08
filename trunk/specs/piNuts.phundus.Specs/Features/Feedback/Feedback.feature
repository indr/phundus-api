Funktionalität: Feedback

@isSmoker
Szenario: Send as anon
	Angenommen not logged in
	Wenn submit feedback as anon with comment:
		"""
		Greetings from feedback feature scenario "Send as anon"!
		
		Server URL: {AppSettings.ServerUrl}
		Version: {Assembly.Version}
		"""
	Dann anon should receive email "Vielen Dank fürs Feedback"
	Und "admin@test.phundus.ch" should receive email "[phundus] Feedback"

