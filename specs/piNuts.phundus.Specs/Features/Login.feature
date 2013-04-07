Funktionalität: Login
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Grundlage:
	Angenommen ich bin nicht angemeldet


Szenario: Unbekannter Benutzer
	Angenommen ich bin auf der Loginseite
	Und ich tippe ins Feld "E-Mail-Adresse" "gibt-es-nicht@test.phundus.ch" ein
	Und ich tippe ins Feld "Passwort" "1234" ein
	Wenn ich auf "Anmelden" klicke
	Dann muss die Meldung "Benutzername oder Passwort inkorrekt." erscheinen


Szenario: Erfolgreiches Login
	Angenommen ich bin auf der Loginseite
	Und ich tippe ins Feld "E-Mail-Adresse" "user@test.phundus.ch" ein
	Und ich tippe ins Feld "Passwort" "1234" ein
	Wenn ich auf "Anmelden" klicke
	Dann sollte ich als "user@test.phundus.ch" angemeldet sein
	Und ich sollte auf der Shopseite sein