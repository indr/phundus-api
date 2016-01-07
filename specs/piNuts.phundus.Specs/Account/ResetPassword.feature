Funktionalität: Password zurücksetzen
	Wenn ein Benutzer sein Passwort nicht mehr weiss, dann kann er ein neues Passwort beantragen.
	Dazu wird dem Benutzer ein E-Mail gesendet, welches einen Link enthält. Wird dieser Link
	aufgerufen, so wird das neue Passwort aktiviert.

Grundlage: 
	Angenommen bestätigter Benutzer

Szenario: Happy Path
	Wenn Passwort zurücksetzen
	Dann E-Mail "[phundus] Neues Passwort" an Benutzer
