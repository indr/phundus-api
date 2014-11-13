
angular.module('ph.inventory', ['ph.resources', 'ph.domain', 'ph.ui', 'ui', 'ui.bootstrap'])
    .config(function($routeProvider) {
    $routeProvider
        .when('/articles/:articleId/stocks', { controller: StocksCtrl, templateUrl: '../../clientviews/inventory/stocks.html' });
}); // ph.inventory

function StocksCtrl($scope, $routeParams, $location) {
    $scope.articleId = $routeParams.articleId;


    $scope.openOld = function(name) {
        alert(name);

        //$location.url(};
    };
}; // StockCtrl