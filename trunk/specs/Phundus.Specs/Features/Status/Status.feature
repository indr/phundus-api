Feature: Status
	
@isSmoker
Scenario: Status
	When I get status info
	Then it should have server url according to App.config
	And it should have server date time within 3 seconds
	And it should have server version according to specs assembly version