Feature: Index
	
@isSmoker
Scenario: Index returns OK
	When I try to request /
	Then I should get status OK
	And I should get content containing "browsehappy"

