Funktionalität: Feedback

Szenario: Send as anon
	Angenommen not logged in
	Wenn submit feedback as anon
	Dann anon should receive email "Vielen Dank fürs Feedback"
	Und "admin@test.phundus.ch" should receive email "[phundus] Feedback"

