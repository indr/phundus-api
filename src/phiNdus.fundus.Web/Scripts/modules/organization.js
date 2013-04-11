﻿angular
    .module('organization', ['phundus-api', 'ui', 'ui.bootstrap'])
    .filter('replace', function () {
        return function (input, pattern, replace) {
            if (input == null)
                return null;
            return input.replace(new RegExp(pattern, 'mg'), replace);
        };
    })
    .config(function ($routeProvider) {
        $routeProvider
            .when('/search', { controller: SearchCtrl, templateUrl: './Content/Views/Organization/Search.html' })
            .when('/establish', { controller: EstablishCtrl, templateUrl: './Content/Views/Organization/Establish.html' })
            .when('/members', { controller: MembersCtrl, templateUrl: './Content/Views/Organization/Members.html' })
            .when('/settings', { controller: SettingsCtrl, templateUrl: './Content/Views/Organization/Settings.html' })
            .otherwise({ redirectTo: '/search' });
    });



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
            member.$update({ org: $scope.organizationId, action: 'setrole' },
            function (data, putResponseHeaders) {
                member.role = roleValue;
            },
            function (err) {
                var msg = err.data.message;
                if (err.data.exceptionMessage != undefined)
                    msg += "\n\n" + err.data.exceptionMessage;
                if (err.data.messageDetail != undefined)
                    msg += "\n\n" + err.data.messageDetail;


                alert('Fehler: ' + "\n\n" + msg);
            });
        }
    };

    $scope.approve = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich bestätigen?')) {
            member.$update({ org: $scope.organizationId, action: 'approve' },
            function (data, putResponseHeaders) {
                member.isApproved = true;
            },
            function (err) {
                var msg = err.data.message;
                if (err.data.exceptionMessage != undefined)
                    msg += "\n\n" + err.data.exceptionMessage;
                if (err.data.messageDetail != undefined)
                    msg += "\n\n" + err.data.messageDetail;


                alert('Fehler: ' + "\n\n" + msg);
            });
        }
    };

    $scope.lock = function (member) {
        if (confirm('Möchen Sie "' + member.firstName + ' ' + member.lastName + '" wirklich sperren?')) {
            member.$update({ org: $scope.organizationId, action: 'lock' },
            function (data, putResponseHeaders) {
                member.isLocked = true;
            },
            function (err) {
                var msg = err.data.message;
                if (err.data.exceptionMessage != undefined)
                    msg += "\n\n" + err.data.exceptionMessage;
                if (err.data.messageDetail != undefined)
                    msg += "\n\n" + err.data.messageDetail;


                alert('Fehler: ' + "\n\n" + msg);
            });
        }
    };

    $scope.unlock = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich entsperren?')) {
            member.$update({ org: $scope.organizationId, action: 'unlock' },
            function (data, putResponseHeaders) {
                member.isLocked = false;
            },
            function (err) {
                var msg = err.data.message;
                if (err.data.exceptionMessage != undefined)
                    msg += "\n\n" + err.data.exceptionMessage;
                if (err.data.messageDetail != undefined)
                    msg += "\n\n" + err.data.messageDetail;


                alert('Fehler: ' + "\n\n" + msg);
            });
        }
    };
}