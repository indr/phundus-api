Funktionalität: Status

@isSmoker
Szenario: /api/v1/status
	Wenn /api/v1/status aufgerufen wird
	Dann muss der Http-Status 200 sein
	Und JSON status: 'OK'
