angular
    .module('members', ['phundus-api'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/', { controller: ListCtrl, templateUrl: '../Content/Members/list.html' })
            .otherwise({ redirectTo: '/' });
    });


function ListCtrl($scope, members) {
    $scope.members = members.query({ org: $scope.organizationId });
    $scope.order = 'lastName';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };

    $scope.setRole = function (member, roleName, roleValue) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich die Rolle "' + roleName + '" geben?')) {
            member.role = roleValue;
            member.$update({ org: $scope.organizationId });
        }
    };

    $scope.lock = function (member) {
        if (confirm('Möchen Sie "' + member.firstName + ' ' + member.lastName + '" wirklich sperren?')) {
            member.isLocked = true;
            member.$update({ org: $scope.organizationId });
        }
    };

    $scope.unlock = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich entsperren?')) {
            member.isLocked = false;
            member.$update({ org: $scope.organizationId });
        }
    };
}