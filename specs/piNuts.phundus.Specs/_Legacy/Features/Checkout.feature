Feature: Checkout
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given ich bin als Benutzer angemeldet
	And mein Warenkorb ist leer

@ignore
Scenario: Checkout 
	Given ich lege den Artikel mit der Id 10022 in den Warenkorb
	And ich lege den Artikel mit der Id 10023 in den Warenkorb
	When ich bestelle
	Then muss "chief-1@test.phundus.ch" ein E-Mail erhalten mit dem Betreff "[phundus] Neue Bestellung"
	And muss "chief-2@test.phundus.ch" ein E-Mail erhalten mit dem Betreff "[phundus] Neue Bestellung"

