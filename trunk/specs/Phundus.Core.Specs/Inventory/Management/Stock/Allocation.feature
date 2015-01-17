Feature: Allocation
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given stock created "Stock-1", article 10001, organization 1001
	And quantity in inventory increased of 10 to 10 as of 07.01.2015

Scenario: Allocation with sufficient quantity in inventory
	When allocate 1 with id 1 for reservation 2 from 07.01.2015 to 08.01.2015
	Then stock allocated 1, Allocated
	And allocations
	| AllocationId | ReservationId | FromUtc    | ToUtc      | Quantity |
	| 1            | 2             | 07.01.2015 | 08.01.2015 | 1        |
	And quantity available changed of -1 to 9 as of 07.01.2015
	And quantity available changed of +1 to 10 as of 08.01.2015
	And quantities available
	| AsOfUtc    | Change | Total |
	| 07.01.2015 | 9      | 9     |
	| 08.01.2015 | 1      | 10    |

Scenario: Allocation with insufficient quantity in inventory
	When allocate 11 with id 2 for reservation 3 from 07.01.2015 to 08.01.2015
	Then stock allocated 2, Unavailable
	And allocations
	| AllocationId | ReservationId | FromUtc    | ToUtc      | Quantity |
	| 2            | 3             | 07.01.2015 | 08.01.2015 | 11       |
	And quantity available changed of -11 to -1 as of 07.01.2015
	And quantity available changed of +11 to 10 as of 08.01.2015
	And quantities available
	| AsOfUtc    | Change | Total |
	| 07.01.2015 | -1     | -1    |
	| 08.01.2015 | 11     | 10    |

Scenario: Change allocation quantity
	Given article allocated, allocation id 1, reservation id 2, from 07.08.2015, to 08.01.2015, quantity 10
	When change allocation 1 quantity 11
	Then allocation 1 quantity changed from 10 to 11

