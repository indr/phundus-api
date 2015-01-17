Feature: ReservationStatusSaga

Scenario: Stock allocated dispatches no command
	When stock allocated
	Then no commands dispatched

Scenario: Order approved dispatches no command
	When order approved
	Then no commands dispatched
