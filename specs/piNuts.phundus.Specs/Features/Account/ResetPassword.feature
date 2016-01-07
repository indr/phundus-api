Funktionalität: Reset password
	Wenn ein Benutzer sein Passwort nicht mehr weiss, dann kann er ein neues Passwort beantragen.
	Dazu wird dem Benutzer ein E-Mail gesendet, welches einen Link enthält. Wird dieser Link
	aufgerufen, so wird das neue Passwort aktiviert.

Grundlage: 
	Angenommen a confirmed user

Szenario: sends email "Neues Passwort"
	Wenn reset password
	Dann user should receive email "[phundus] Neues Passwort"
