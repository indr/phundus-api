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
            .when('/applications', {controller: ApplicationsCtrl, templateUrl: './Content/Views/Organization/Applications.html' })
            .when('/settings', { controller: SettingsCtrl, templateUrl: './Content/Views/Organization/Settings.html' })            
            .when('/files', { controller: FilesCtrl, templateUrl: './Content/Views/Organization/Files.html'})
            .otherwise({ redirectTo: '/search' });
    });


    function FilesCtrl($scope) {

        $('#fileupload').attr("action", "./orgs/" + $scope.organizationId + "/files");


        // Initialize the jQuery File Upload widget:
        $('#fileupload').fileupload();

        // Enable iframe cross-domain access via redirect option:
        $('#fileupload').fileupload(
            'option',
            'redirect',
            window.location.href.replace(
                /\/[^\/]*$/,
                '/cors/result.html?%s'
            )
        );

        // Load existing files:
        $('#fileupload').each(function () {
            var that = this;
            $.getJSON(this.action, function (result) {
                if (result && result.length) {
                    $(that).fileupload('option', 'done')
                        .call(that, null, { result: result });
                }
            });
        });
    }
    
    function HomeCtrl($scope, applications) {
        $scope.join = function () {
            
            
            if (!confirm('Möchten Sie dieser Organisation beitreten?'))
                return;

            applications.save({ organizationId: $scope.organizationId },
            function (data, putResponseHeaders) {
                alert('Ein Beitrittsgesuch wurde platziert.');
            },
            function (err) {
                alert('Fehler: ' + err.data.exceptionMessage);
            });

        };
    }

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

function SettingsCtrl($scope, organizations, files) {
    $scope.alerts = [];

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.settings = organizations.get({ id: $scope.organizationId });
    //$scope.fileNames = [{ "_": null, "url": "/phundus-debug/Content/Uploads/Organizations/1001/Einsatzdoku_NLZ.docx", "thumbnail_url": "/phundus-debug/Content/Uploads/Organizations/1001/Einsatzdoku_NLZ.docx.ashx?maxwidth=80&maxheight=80", "name": "Einsatzdoku_NLZ.docx", "type": ".docx", "size": 14913, "delete_url": "/phundus-debug/orgs/1001/Files/Delete/Einsatzdoku_NLZ.docx", "delete_type": "DELETE" }, { "_": null, "url": "/phundus-debug/Content/Uploads/Organizations/1001/IMG_3019.JPG", "thumbnail_url": "/phundus-debug/Content/Uploads/Organizations/1001/IMG_3019.JPG.ashx?maxwidth=80&maxheight=80", "name": "IMG_3019.JPG", "type": ".JPG", "size": 3682475, "delete_url": "/phundus-debug/orgs/1001/Files/Delete/IMG_3019.JPG", "delete_type": "DELETE" }, { "_": null, "url": "/phundus-debug/Content/Uploads/Organizations/1001/IMG_3021.JPG", "thumbnail_url": "/phundus-debug/Content/Uploads/Organizations/1001/IMG_3021.JPG.ashx?maxwidth=80&maxheight=80", "name": "IMG_3021.JPG", "type": ".JPG", "size": 4075054, "delete_url": "/phundus-debug/orgs/1001/Files/Delete/IMG_3021.JPG", "delete_type": "DELETE" }, { "_": null, "url": "/phundus-debug/Content/Uploads/Organizations/1001/IMG_3022.JPG", "thumbnail_url": "/phundus-debug/Content/Uploads/Organizations/1001/IMG_3022.JPG.ashx?maxwidth=80&maxheight=80", "name": "IMG_3022.JPG", "type": ".JPG", "size": 3330802, "delete_url": "/phundus-debug/orgs/1001/Files/Delete/IMG_3022.JPG", "delete_type": "DELETE" }];


    $scope.files = files.query({ orgId: $scope.organizationId });

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

function ApplicationsCtrl($scope, applications, members) {
    $scope.applications = applications.query({ organizationId: $scope.organizationId });
    $scope.order = 'lastName';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };

    $scope.approve = function (application) {
        if (confirm('Möchten Sie "' + application.firstName + ' ' + application.lastName + '" wirklich bestätigen?')) {
            members.save({ organizationId: $scope.organizationId }, {applicationId: application.id },
            function (data, putResponseHeaders) {
                alert("Die Mitgliedschaft wurde bestätigt.");
                //application.isApproved = true;
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

    $scope.reject = function(application) {
        if (!confirm('Möchten Sie "' + application.firstName + ' ' + application.lastName + '" wirklich ablehnen?'))
            return;

        application.$delete();
    };
}

function MembersCtrl($scope, members) {
    $scope.members = members.query({ organizationId: $scope.organizationId });

    $scope.order = 'lastName';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };

    $scope.setRole = function (member, roleName, roleValue) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich die Rolle "' + roleName + '" geben?')) {
            member.$update({ organizationId: $scope.organizationId, action: 'setrole' },
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
            member.$update({ organizationId: $scope.organizationId, action: 'approve' },
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
            member.$update({ organizationId: $scope.organizationId, action: 'lock' },
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
            member.$update({ organizationId: $scope.organizationId, action: 'unlock' },
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