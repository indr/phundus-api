﻿Funktionalität: Feedback
	Um den machern von phundus etwas mitzuteilen
	Als ein Benutzer
	Will ich ein Feedbackformular ausfüllen


Szenario: Feedback ohne Angabe der E-Mail-Adresse
	Angenommen ich bin auf der Feedbackseite
	Und ich tippe ins Feld "Feedback" "Hallo" ein
	Wenn ich auf "Senden" drücke
	Dann muss die Meldung "Das Feld "E-Mail-Adresse" ist erforderlich." erscheinen
	Und muss das Feld "E-Mail-Adresse" rot sein


Szenario: Feedback ohne Angabe des Feedbacks
	Angenommen ich bin auf der Feedbackseite
	Und ich tippe ins Feld "E-Mail-Adresse" "user@example.com" ein
	Wenn ich auf "Senden" drücke
	Dann muss die Meldung "Das Feld "Feedback" ist erforderlich." erscheinen
	Und muss das Feld "Feedback" rot sein


@isSmoker
Szenario: Feedback senden
	Angenommen ich bin auf der Feedbackseite
	Und ich tippe ins Feld "E-Mail-Adresse" "user@test.phundus.ch" ein
	Und ich füge ins Feld "Feedback" ein:
		"""
		Grüsse vom Feedback-Feature-Szenario "Feedback senden"!
		
		Server-Url: {AppSettings.ServerUrl}
		Version: {Assembly.Version}
		"""
	Wenn ich auf "Senden" drücke
	Dann muss die Meldung "Merci!" erscheinen
	Und muss "user@test.phundus.ch" ein E-Mail erhalten mit dem Betreff "Vielen Dank fürs Feedback" und dem Text:
		"""
		Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

		Vielen Dank und freundliche Grüsse

		Das phundus-Team
		"""
	Und muss "admin@test.phundus.ch" ein E-Mail erhalten mit dem Betreff "[phundus] Feedback"
