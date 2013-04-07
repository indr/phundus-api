angular.module('members', ['mongolab']).
    config(function ($routeProvider) {
        $routeProvider
            .when('/', { controller: ListCtrl, templateUrl: '../Content/Members/list.html' })
            .when('/edit/:projectId', { controller: EditCtrl, templateUrl: '../Content/Members/detail.html' })
            .when('/new', { controller: CreateCtrl, templateUrl: '../Content/Members/detail.html' })
            .otherwise({ redirectTo: '/' });
    });


function ListCtrl($scope, Members) {
    $scope.members = Members.query({ org: $scope.organizationId});
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
            member.$update();
        }
    };

    $scope.lock = function (member) {
        if (confirm('Möchen Sie "' + member.firstName + ' ' + member.lastName + '" wirklich sperren?')) {
            member.isLocked = true;
            member.$update();
        }
    };

    $scope.unlock = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich entsperren?')) {
            member.isLocked = false;
            member.$update();
        }
    };
}


function CreateCtrl($scope, $location, Members) {
    $scope.save = function() {
        Members.save($scope.project, function (project) {
            $location.path('/edit/' + project._id.$oid);
        });
    };
}


function EditCtrl($scope, $location, $routeParams, Members) {
    var self = this;

    Members.get({ id: $routeParams.projectId }, function (project) {
        self.original = project;
        $scope.project = new Members(self.original);
    });

    $scope.isClean = function() {
        return angular.equals(self.original, $scope.project);
    };

    $scope.destroy = function () {
        self.original.destroy(function () {
            $location.path('/list');
        });
    };

    $scope.save = function () {
        $scope.project.update(function () {
            $location.path('/');
        });
    };
}