Funktionalität: Checkout
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Grundlage: 
	Angenommen ich bin als Benutzer angemeldet
	Und mein Warenkorb ist leer


Szenario: Checkout 
	Angenommen ich lege den Artikel mit der Id 10001 in den Warenkorb
	Und ich lege den Artikel mit der Id 10002 in den Warenkorb
	Wenn ich bestelle
	Dann muss "chief-1@test.phundus.ch" ein E-Mail erhalten mit dem Betreff "[phundus] Neue Bestellung"
	Und muss "chief-2@test.phundus.ch" ein E-Mail erhalten mit dem Betreff "[phundus] Neue Bestellung"

