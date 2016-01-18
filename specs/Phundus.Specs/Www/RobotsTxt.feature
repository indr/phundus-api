Feature: RobotsTxt

@isSmoker
Scenario: robots.txt returns OK
	When I try to request /robots.txt
	Then I should get status OK
