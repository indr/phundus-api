Funktionalität: LogIn

Grundlage: 
	Angenommen not logged in

Szenario: Log in successful
	Angenommen a confirmed user
	Wenn log in
	Dann logged in

Szenario: Log in failed cause of lock
	Angenommen a confirmed, locked user
	Und lock user
	Wenn log in
	Dann not logged in

Szenario: Log in successful after unlock
	Angenommen a confirmed, locked user
	Und unlock user
	Wenn log in
	Dann logged in