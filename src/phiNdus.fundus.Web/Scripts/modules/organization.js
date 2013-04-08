angular
    .module('organization', ['phundus-api', 'ui', 'ui.bootstrap'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/members', { controller: MembersCtrl, templateUrl: './Content/Views/Organization/Members.html' })
            .when('/settings', { controller: SettingsCtrl, templateUrl: './Content/Views/Organization/Settings.html' })
            .otherwise({ redirectTo: '/' });
    });


function SettingsCtrl($scope, organizations) {
    $scope.alerts = [];

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.settings = organizations.get({ id: $scope.organizationId });

    $scope.reset = function () {
        $scope.alerts.length = 0;
        $scope.settings = organizations.get({ id: $scope.organizationId });
    };

    $scope.save = function () {
        $scope.alerts.length = 0;
        $scope.alerts.push({ type: 'info', msg: 'Die Änderungen werden gespeichert...' });
        $scope.settings.$update({},
            function (data, putResponseHeaders) {
                $scope.alerts.length = 0;
                $scope.alerts.push({ type: 'success', msg: 'Die Änderungen wurden erfolgreich gespeichert.' });
            },
            function (err) {
                $scope.alerts.length = 0;
                $scope.alerts.push({ type: 'error', msg: 'Fehler: ' + err.data.exceptionMessage });
            });
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