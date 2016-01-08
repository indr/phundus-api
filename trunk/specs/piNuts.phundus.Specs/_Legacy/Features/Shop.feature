Feature: Shop
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given Ich bin auf der Startseite
	Given ich bin nicht angemeldet

Scenario: Artikeldetails
	When ich wähle den Artikel 10027 aus
	Then muss der Artikel geöffnet sein
