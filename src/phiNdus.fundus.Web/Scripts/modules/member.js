angular.module('members', ['ngResource']).
config(function ($routeProvider) {
    $routeProvider
        .when('/organization/members', { controller: ListCtrl })
        //.when('/edit/:projectId', { controller: EditCtrl, templateUrl: 'detail.html' })
        //.when('/new', { controller: CreateCtrl, templateUrl: 'detail.html' })
        .otherwise({ redirectTo: '/' });
});

function ListCtrl($scope, Members) {
    $scope.members =
    [
        {
            firstName: 'hallo',
            lastName: 'test'
        }
    ,
        {
            firstName: 'hallo1',
            lastName: 'test1'
        }
    ];


}