Feature: AllocationSaga

Scenario: Article reserved dispatches allocate stock
	When article reserved
	Then allocate stock

Scenario: Reservation cancelled dispatches remove allocation
	Given article reserved
	When reservation cancelled
	Then discard allocation

Scenario: Reservation period changed dispatches change allocation period
	Given article reserved
	When reservation period changed
	Then change allocation period

Scenario: Reservation quantity changed dispatches change allocation quantity
	Given article reserved
	When reservation quantity changed
	Then change allocation quantity