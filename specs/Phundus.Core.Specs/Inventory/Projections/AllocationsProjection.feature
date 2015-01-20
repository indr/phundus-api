Feature: AllocationsProjection

Background: 
	Given stock created "Stock1", article 10001, organization 1001

Scenario: Stock allocated sets allocation status
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1, status Allocated
	When I ask for allocations
	Then allocation data
	| AllocationId | ReservationId | AllocationStatus |
	| 1            | 2             | Allocated        |
