
angular.module('ph.inventory', ['ph.resources', 'ph.domain', 'ph.ui', 'ui', 'ui.bootstrap'])
    .config(function($routeProvider) {
    $routeProvider
        .when('/articles/:articleId/in-inventory', { controller: InInventoryCtrl, templateUrl: '../clientviews/inventory/ininventory.html' })
        .when('/articles/:articleId/availabilities', { controller: AvailabilitiesCtrl, templateUrl: '../clientviews/inventory/availabilities.html' })
        .when('/articles/:articleId/allocations', { controller: AllocationsCtrl, templateUrl: '../clientviews/inventory/allocations.html' })
        .when('/articles/:articleId/reservations', {controller: ReservationsCtrl, templateUrl: '../clientviews/inventory/reservations.html'});
}); // ph.inventory

function ReservationsCtrl($scope, $routeParams, reservations) {
    $scope.isLoading = true;
    $scope.articleId = $routeParams.articleId;

    $scope.reservations = reservations.query({ organizationId: $scope.organizationId, articleId: $scope.articleId },
       function (data) { $scope.isLoading = false; }, function () { $scope.isLoading = false; });
    
}; // ReservationsCtrl

function AvailabilitiesCtrl($scope, $routeParams, availabilities) {
    $scope.isLoading = true;
    $scope.articleId = $routeParams.articleId;

    $scope.availabilities = availabilities.query({ organizationId: $scope.organizationId, articleId: $scope.articleId },
       function (data) { $scope.isLoading = false; }, function () { $scope.isLoading = false; });

}; // AvailabilitiesCtrl

function AllocationsCtrl($scope, $routeParams, allocations) {
    $scope.isLoading = true;
    $scope.articleId = $routeParams.articleId;

    $scope.allocations = allocations.query({ organizationId: $scope.organizationId, articleId: $scope.articleId },
       function (data) { $scope.isLoading = false; }, function () { $scope.isLoading = false; });

}; // AvailabilitiesCtrl

function InInventoryCtrl($scope, $routeParams, stocks, quantitiesInInventory) {
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

    $scope.newItem = {
        comment: 'Manuelle Bestandesänderung', change: 1, asOfUtc: new Date(),
        organizationId: $scope.organizationId, articleId: $scope.articleId, stockId: $scope.stockId
    };

    $scope.showChangeQuantity = function () {
        $('#modal-change-quantity').modal('show');
    };

    $scope.changeQuantity = function() {

        quantitiesInInventory.save({ organizationId: $scope.organizationId, articleId: $scope.articleId, stockId: $scope.stockId },
        $scope.newItem, function() {

            $scope.queryQuantitiesInInventory();
        }, function () { });
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