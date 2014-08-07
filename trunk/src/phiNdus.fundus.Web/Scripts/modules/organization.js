
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

function HomeCtrl($scope, relationships, applications) {

    $scope.relationship = relationships.get({ 'organizationId': $scope.organizationId });


    $scope.join = function () {
        if (!confirm('Möchten Sie dieser Organisation wirklich beitreten?'))
            return;

        applications.save({ organizationId: $scope.organizationId },
        function (data, putResponseHeaders) {
            $scope.relationship = {
                statusString: 'Application',
                dateTime: new Date()
            };

            var $div = $('#modal-show-message');
            $div.find('.modal-header h3').html("Hinweis");
            $div.find('.modal-body').html("<p>Die Mitgliedschaft wurde beantragt.</p><p>Du erhältst ein E-Mail wenn ein Administrator diese bestätigt oder ablehnt.");
            $div.modal();
        });

    };
}