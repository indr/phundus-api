Funktionalität: Startseite

# @isSmoker
# Szenario: Aufruf der Startseite
# 	Wenn ich die Webseite aufrufe
# 	Dann sollte ich ein Heading 1 mit "'Allo, 'Allo!" sehen

@isSmoker
Szenario: Aufruf des Shops
	Wenn ich den Shop aufrufe
	Dann sollte ich gross "Shop" sehen
	Und sollte im Fenstertitel muss "Shop - phundus" stehen

@isSmoker
Szenario: Korrekte Version wurde installiert
	Wenn ich den Shop aufrufe
	Dann sollte die Version entsprechend der zuletzt installierten Version sein

@isSmoker
@ignore
Szenario: Server-URL wurde hinterlegt
	Wenn ich den Shop aufrufe
	Dann sollte die Server-URL entsprechend der Konfiguration gesetzt sein

