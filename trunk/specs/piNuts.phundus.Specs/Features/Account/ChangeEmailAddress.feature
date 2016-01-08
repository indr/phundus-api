Funktionalität: ChangeEmailAddress
	
Grundlage: 
	Angenommen a confirmed user
	Und logged in as user

Szenario: sends email "Validierung der geänderten E-Mail-Adresse"
	Wenn change email address
	Dann user should receive email "[phundus] Validierung der geänderten E-Mail-Adresse" at requested address
