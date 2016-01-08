Feature: Startseite

# @isSmoker
# Scenario: Aufruf der Startseite
# 	When ich die Webseite aufrufe
# 	Then sollte ich ein Heading 1 mit "'Allo, 'Allo!" sehen

@isSmoker
Scenario: Aufruf des Shops
	When ich den Shop aufrufe
#	Then sollte ich gross "Shop" sehen
	Then sollte im Fenstertitel muss "Shop - phundus" stehen

@isSmoker
Scenario: Korrekte Version wurde installiert
	When ich den Shop aufrufe
	Then sollte die Version entsprechend der zuletzt installierten Version sein

@isSmoker
@ignore
Scenario: Server-URL wurde hinterlegt
	When ich den Shop aufrufe
	Then sollte die Server-URL entsprechend der Konfiguration gesetzt sein

