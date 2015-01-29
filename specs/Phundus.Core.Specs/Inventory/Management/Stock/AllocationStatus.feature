Feature: AllocationStatus	

Background: 
	Given stock created "Stock1", article 10001, organization 1001
	And quantity in inventory increased of 10 to 10 as of 01.02.2015
	And quantity available changed from 01.02.2015 of 10

Scenario: Stock allocated with quantity not available changes allocation status to unavailable
	When allocate stock, allocation id 2, reservation id 3, from 01.01.2015 to 08.01.2015, quantity 1
	Then any allocation status changed, allocation id 2, new status Unavailable
	And allocations
	| AllocationId | Status      |
	| 2            | Unavailable |

Scenario: Stock allocated with quantity available changes allocation status to allocated
	When allocate stock, allocation id 2, reservation id 3, from 01.02.2015 to 08.02.2015, quantity 6
	Then any allocation status changed, allocation id 2, new status Allocated
	And allocations
	| AllocationId | Status    |
	| 2            | Allocated |

Scenario: Two allocations with no availabilities for both
	When allocate stock, allocation id 1, reservation id 3, from 01.02.2015 to 08.02.2015, quantity 6
	And allocate stock, allocation id 2, reservation id 3, from 01.02.2015 to 08.02.2015, quantity 6
	Then allocations
	| AllocationId | Status      |
	| 1            | Allocated   |
	| 2            | Unavailable |

Scenario: Increasement of quantity in inventory changes allocation status from unavailble to allocated
	
