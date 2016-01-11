Feature: Startseite

@isSmoker
Scenario: Aufruf des Shops
	When ich den Shop aufrufe
	Then sollte im Fenstertitel muss "Shop - phundus" stehen

@isSmoker
Scenario: Korrekte Version wurde installiert
	When ich den Shop aufrufe
	Then sollte die Version entsprechend der zuletzt installierten Version sein


