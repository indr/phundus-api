
angular.module('ph.documents', ['ph.domain', 'ph.resources', 'ph.ui', 'ui', 'ui.bootstrap'])
    .config(function($routeProvider) {
        $routeProvider
            .when('/orders', { controller: OrdersCtrl, templateUrl: './Content/Views/Documents/Orders.html' })
            .when('/orders/:orderId', { controller: OrderCtrl, templateUrl: './Content/Views/Documents/Order.html' })
            .when('/contracts', { controller: ContractsCtrl, templateUrl: './Content/Views/Documents/Contracts.html' })
            .when('/contracts/:contractId', { controller: ContractCtrl, templateUrl: './Content/Views/Documents/Contract.html' });

    })
; // ph.documents

function OrdersCtrl($scope, $location, orders) {
    $scope.isLoading = true;
    $scope.orders = orders.query(
        function () { $scope.isLoading = false; },
        function () { $scope.isLoading = false; });

    $scope.search = { status: '' };
    $scope.order = '-createdOn';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };

    $scope.openOrder = function (order) {
        $location.path('/orders/' + order.orderId);
    };
};

function OrderCtrl($scope, $routeParams, orders) {
    $scope.isLoading = true;
    $scope.isLoaded = false;
    $scope.order = orders.get({"orderId": $routeParams.orderId },
        function () { $scope.isLoading = false; $scope.isLoaded = true; },
        function () { $scope.isLoading = false; });
};

function ContractsCtrl($scope, $location, contracts) {
    $scope.isLoading = true;
    $scope.contracts = contracts.query(
        function () { $scope.isLoading = false; },
        function () { $scope.isLoading = false; });

    $scope.search = { status: '' };
    $scope.order = '-createdOn';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };

    $scope.openContract = function (contract) {
        $location.path('/contracts/' + contract.contractId);
    };
};

function ContractCtrl($scope, $routeParams, contracts) {
    $scope.isLoading = true;
    $scope.isLoaded = false;
    $scope.order = contracts.get({ "contract": $routeParams.contractId },
        function () { $scope.isLoading = false; $scope.isLoaded = true; },
        function () { $scope.isLoading = false; });
};