Funktionalität: Status

@isSmoker
Szenario: /node/status
	Wenn /node/status aufgerufen wird
	Dann muss der Http-Status 200 sein
	Und JSON status: 'OK'
