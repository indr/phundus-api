
angular.module('organization', ['phundus-api', 'ui', 'ui.bootstrap'])

    .filter('replace', function () {
        return function(input, pattern, replace) {
            if (input == null)
                return null;
            return input.replace(new RegExp(pattern, 'mg'), replace);
        };
    })

    .config(function ($routeProvider) {
        $routeProvider
            .when('/search', { controller: SearchCtrl, templateUrl: './Content/Views/Organization/Search.html' })
            .when('/establish', { controller: EstablishCtrl, templateUrl: './Content/Views/Organization/Establish.html' })
            .when('/', { controller: HomeCtrl, templateUrl: './Content/Views/Organization/Home.html' });
    })
;

function SearchCtrl($scope, organizations) {
    $scope.organizations = organizations.query();
    $scope.order = 'name';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };
}

function EstablishCtrl($scope) {
    
}

function HomeCtrl($scope) {
    
}