Feature: AllocationsProjection

Background: 
	Given stock created "Stock1", article 10001, organization 1001

Scenario: Stock allocated sets allocation status
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	Then all allocations by article id
	| AllocationId | ReservationId | Quantity | AllocationStatus |
	| 1            | 2             | 1        | Unknown          |

Scenario: Allocation quantity changed updates projection
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	And allocation quantity changed, allocation id 1, new quantity 2
	Then all allocations by article id
	| AllocationId | Quantity |
	| 1            | 2        |

Scenario: Allocation period changed updates projection
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	And allocation period changed, allocation id 1, new from 09.01.2015 to 13.01.2015
	Then all allocations by article id
	| AllocationId | FromUtc    | ToUtc      |
	| 1            | 09.01.2015 | 13.01.2015 |

Scenario: Allocation discarded
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	And allocation discarded, allocation id 1
	Then all allocations by article id
	| AllocationId | FromUtc | ToUtc |

Scenario: Allocation status changed
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	And allocation status changed, allocation id 1, new status Allocated
	Then all allocations by article id
	| AllocationId | AllocationStatus |
	| 1            | Allocated        |
	