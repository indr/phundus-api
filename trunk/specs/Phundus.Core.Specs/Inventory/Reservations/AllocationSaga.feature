Feature: AllocationSaga

Scenario: Article reserved dispatches allocate stock
	When article reserved
	Then allocate stock
