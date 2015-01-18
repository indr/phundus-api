Feature: Allocation

Background: 
	Given stock created "Stock-1", article 10001, organization 1001
	And quantity in inventory increased of 10 to 10 as of 07.01.2015

Scenario: Allocate stock publishes stock allocated and updates allocations
	When allocate stock, allocation id 2, reservation id 3, from 07.01.2015 to 08.01.2015, quantity 11
	Then stock allocated 2
	And allocations
	| AllocationId | ReservationId | FromUtc    | ToUtc      | Quantity |
	| 2            | 3             | 07.01.2015 | 08.01.2015 | 11       |

Scenario: Allocate stock changes availability
	When allocate stock, allocation id 2, reservation id 3, from 07.01.2015 to 08.01.2015, quantity 11
	Then stock allocated 2
	And quantity available changed of -11 to -1 as of 07.01.2015
	And quantity available changed of +11 to 10 as of 08.01.2015
	And quantities available
	| AsOfUtc    | Change | Total |
	| 07.01.2015 | -1     | -1    |
	| 08.01.2015 | 11     | 10    |

Scenario: Change allocation quantity updates allocations
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	When change allocation quantity, allocation id 1, new quantity 3
	Then allocation quantity changed, allocation id 1, new quantity 3
	And allocations
	| AllocationId | ReservationId | FromUtc    | ToUtc      | Quantity |
	| 1            | 2             | 07.01.2015 | 08.01.2015 | 3        |

Scenario: Change allocation quantity updates availabilities
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	When change allocation quantity, allocation id 1, new quantity 3
	Then allocation quantity changed, allocation id 1, new quantity 3
	And quantity available changed of -2 to 7 as of 07.01.2015
	And quantity available changed of +2 to 10 as of 08.01.2015
	And quantities available
	| AsOfUtc    | Change | Total |
	| 07.01.2015 | -3     | 7     |
	| 08.01.2015 | +3     | 10    |

Scenario: Change allocation period updates allocations
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	When change allocation period, allocation id 1, new from 08.01.2015 to 09.01.2015
	Then allocation period changed, allocation id 1, new from 08.01.2015 to 09.01.2015
	And allocations
	| AllocationId | ReservationId | FromUtc    | ToUtc      | Quantity |
	| 1            | 2             | 08.01.2015 | 09.01.2015 | 1        |

Scenario: Change allocation period updates availabilities
	Given stock allocated, allocation id 1, reservation id 2, from 07.01.2015 to 08.01.2015, quantity 1
	When change allocation period, allocation id 1, new from 08.01.2015 to 09.01.2015
	Then allocation period changed, allocation id 1, new from 08.01.2015 to 09.01.2015
	And quantity available changed of +1 to 10 as of 07.01.2015
	And quantity available changed of -1 to 9 as of 08.01.2015
	And quantity available changed of +2 to 10 as of 08.01.2015
	And quantities available
	| AsOfUtc    | Change | Total |
	| 07.01.2015 | +10    | 10    |
	| 08.01.2015 | -1     | 9     |
	| 09.01.2015 | +1     | 10    |