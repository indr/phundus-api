
angular.module('ph.inventory', ['ph.resources', 'ph.domain', 'ph.ui', 'ui', 'ui.bootstrap'])
    .config(function($routeProvider) {
    $routeProvider
        .when('/articles/:articleId/stocks', { controller: StocksCtrl, templateUrl: '../../clientviews/inventory/stocks.html' });
}); // ph.inventory

function StocksCtrl($scope, $routeParams, stocks, quantitiesInInventory) {
    $scope.isLoading = true;
    $scope.articleId = $routeParams.articleId;
    $scope.stockId = null;

    $scope.createStock = function() {
        var stock = { organizationId: $scope.organizationId, articleId: $scope.articleId };
        stocks.save(stock, stock, function(data) {
            $scope.stockId = data.id;

            $scope.queryQuantitiesInInventory();
        }, function() {

        });
    };

    $scope.changeQuantity = function() {
        var change = prompt('Bestand veränderum um', '');
        if (change == null)
            return;

        quantitiesInInventory.save({ organizationId: $scope.organizationId, articleId: $scope.articleId, stockId: $scope.stockId },
        {
            
            change: change
        }, function() { alert('yes'); }, function() { alert('no'); });
    };

    $scope.stocks = stocks.query({ organizationId: $scope.organizationId, articleId: $scope.articleId },
        function(data) {
            $scope.isLoading = false;

            if (data.length > 0) {
                $scope.stockId = data[0].stockId;
                $scope.queryQuantitiesInInventory();
            } else {
                $scope.stockId = null;
            }
        },
        function () { $scope.isLoading = false; });

    $scope.queryQuantitiesInInventory = function() {
        $scope.quantities = quantitiesInInventory.query({ organizationId: $scope.organizationId, articleId: $scope.articleId, stockId: $scope.stockId },
            function() { $scope.isLoading = false;
                $scope.isLoaded = true;
            }, function() { $scope.isLoading = false; });
    }

}; // StocksCtrl