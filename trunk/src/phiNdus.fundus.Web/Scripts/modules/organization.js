angular
    .module('organization', ['phundus-api', 'ui'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/members', { controller: MembersCtrl, templateUrl: './Content/Views/Organization/Members.html' })
            .when('/settings', { controller: SettingsCtrl, templateUrl: './Content/Views/Organization/Settings.html' })
            .otherwise({ redirectTo: '/members' });
    });


function SettingsCtrl($scope, organizations) {
    $scope.settings = organizations.get({ id: $scope.organizationId });

    $scope.reset = function() {
        $scope.settings = organizations.get({ id: $scope.organizationId });
    };

    $scope.save = function () {
        $scope.settings.$update();
    };

}

function MembersCtrl($scope, members) {
    // TODO: org in RouteParams
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
            // TODO: org in RouteParams
            member.$update({ org: $scope.organizationId });
        }
    };

    $scope.lock = function (member) {
        if (confirm('Möchen Sie "' + member.firstName + ' ' + member.lastName + '" wirklich sperren?')) {
            member.isLocked = true;
            // TODO: org in RouteParams
            member.$update({ org: $scope.organizationId });
        }
    };

    $scope.unlock = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich entsperren?')) {
            member.isLocked = false;
            // TODO: org in RouteParams
            member.$update({ org: $scope.organizationId });
        }
    };
}