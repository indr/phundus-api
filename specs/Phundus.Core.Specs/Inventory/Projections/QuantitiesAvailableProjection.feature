Feature: QuantitiesAvailableProjection

Background: 
	Given stock created "Stock1", article 10001, organization 1001
	And stock created "Stock2", article 10001, organization 1001	

Scenario: Two changes not overlapping
	Given quantity available changed from 09.11.2014 to 12.11.2014 of 2 in Stock1
	And quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 09.11.2014 | 2        |
	| Stock1  | 12.11.2014 | 0        |
	| Stock1  | 14.11.2014 | 3        |
	| Stock1  | 16.11.2014 | 0        |

Scenario: Two changes overlapping at the end of first
	Given quantity available changed from 09.11.2014 to 15.11.2014 of 2 in Stock1
	And quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 09.11.2014 | 2        |
	| Stock1  | 14.11.2014 | 5        |
	| Stock1  | 15.11.2014 | 3        |
	| Stock1  | 16.11.2014 | 0        |

Scenario: Two changes overlapping at the start of first
	Given quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	And quantity available changed from 09.11.2014 to 15.11.2014 of 2 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 09.11.2014 | 2        |
	| Stock1  | 14.11.2014 | 5        |
	| Stock1  | 15.11.2014 | 3        |
	| Stock1  | 16.11.2014 | 0        |

Scenario: Two changes with same from date
	Given quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	And quantity available changed from 14.11.2014 to 15.11.2014 of 2 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 14.11.2014 | 5        |
	| Stock1  | 15.11.2014 | 3        |
	| Stock1  | 16.11.2014 | 0        |

Scenario: Two changes with same to date
	Given quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	And quantity available changed from 12.11.2014 to 16.11.2014 of 2 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 12.11.2014 | 2        |
	| Stock1  | 14.11.2014 | 5        |
	| Stock1  | 16.11.2014 | 0        |

Scenario: Two changes enclosing the first
	Given quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	And quantity available changed from 09.11.2014 to 18.11.2014 of 2 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 09.11.2014 | 2        |
	| Stock1  | 14.11.2014 | 5        |
	| Stock1  | 16.11.2014 | 2        |
	| Stock1  | 18.11.2014 | 0        |

Scenario: Two changes inner the first
	Given quantity available changed from 09.11.2014 to 18.11.2014 of 2 in Stock1
	And quantity available changed from 14.11.2014 to 16.11.2014 of 3 in Stock1
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 09.11.2014 | 2        |
	| Stock1  | 14.11.2014 | 5        |
	| Stock1  | 16.11.2014 | 2        |
	| Stock1  | 18.11.2014 | 0        |

Scenario: Multiple stocks support
	Given quantity available changed from 09.11.2014 to 12.11.2014 of 2 in Stock1
	And quantity available changed from 01.12.2015 to 10.12.2015 of 3 in Stock2
	When I ask for quantities available in stock "Stock1"
	Then quantities available data
	| StockId | AsOfUtc    | Quantity |
	| Stock1  | 09.11.2014 | 2        |
	| Stock1  | 12.11.2014 | 0        |



