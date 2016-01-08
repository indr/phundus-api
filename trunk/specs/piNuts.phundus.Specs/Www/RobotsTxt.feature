Feature: RobotsTxt

@isSmoker
Scenario: robots.txt
	When /robots.txt aufgerufen wird
	Then muss der Http-Status 200 sein
