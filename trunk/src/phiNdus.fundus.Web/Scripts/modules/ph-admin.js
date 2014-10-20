
angular.module('ph.admin', ['ph.resources', 'ph.domain', 'ph.ui', 'ui', 'ui.bootstrap'])
    .config(function($routeProvider) {
        $routeProvider
            .when('/schemaupdate', { controller: SchemaUpdateCtrl, templateUrl: './Content/Views/Admin/SchemaUpdate.html' })
            .when('/eventlog', { controller: EventLogCtrl, templateUrl: './Content/Views/Admin/EventLog.html' });
    }); // ph.admin

function SchemaUpdateCtrl($scope, $http, schemaUpdate) {
    $scope.isLoading = true;
    $scope.isLoaded = false;

    $http.get('./api/diagnostics/schema-update').then(function(response) {
        $scope.isLoading = false;
        $scope.isLoaded = true;

        if (response.status == 204)
            $scope.schemaUpdate = "Kein Schema-Update vorhanden.";
        else {
            $scope.schemaUpdate = response.data.script;
        }
    });

    //$scope.schemaUpdate = schemaUpdate.get({}, function(data, xhr) {
    //    $scope.isLoading = false;
    //    $scope.isLoaded = true;
    //    if (xhr.statusCode == 204)
    //        $scope.schemaUpdate = 'leer';
    //}, function() {
    //    $scope.isLoading = false;
    //});

}; // SchemaUpdateCtrl

function EventLogCtrl($scope, eventLog) {
    $scope.isLoading = true;

    $scope.eventLog = eventLog.query({}, function() { $scope.isLoading = false; }, function() { $scope.isLoading = false; });

}; // EventLogCtrl