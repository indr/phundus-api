Feature: StocksProjection
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Simple listing
	Given stock created "Stock-1", article 1001, organization 101
	And stock created "Stock-2", article 1002, organization 101
	And stock created "Stock-3", article 1001, organization 102
	When I ask for all stocks of article 1001
	Then the stocks should be
	| StockId	| ArticleId |
	| "Stock-1" | 1001      |
	| "Stock-3" | 1001      |
