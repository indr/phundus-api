angular.module('phundus-api', ['ngResource'])

    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.responseInterceptors.push('errorMessageHttpInterceptor');
    }])

    .factory('errorMessageHttpInterceptor', function ($q) {
        return function (promise) {
            return promise.then(function (response) {
                return response;
            }, function (response) {

                // TODO: Separation of concerns
                var $div = $('#modal-show-error');
                $div.find('.modal-header h3').html("Fehler " + response.status);
                $div.find('.modal-body').html("<p>" + response.data.exceptionMessage + "</p>");
                $div.modal();

                return $q.reject(response);
            });
        }
    })

    .factory('members', function($resource) {
        var member = $resource(
            './api/:organizationId/members/:id/:action',
            { organizationId: '@organizationId', id: '@id' },
            { update: { method: 'PUT' } }
        );

        return member;
    })
    .factory('organizations', function($resource) {
        var organization = $resource(
            './api/organizations/:id',
            { id: '@id', action: 'update' },
            { update: { method: 'PUT' } }
        );

        return organization;
    })
    .factory('files', function($resource) {
        var files = $resource(
            './orgs/:orgId/files'
        );
        return files;
    })
    .factory('applications', function($resource) {
        return $resource(
            './api/organizations/:organizationId/applications/:id',
            { organizationId: '@organizationId', id: '@id' });
    })
    .factory('membersLocks', function($resource) {
        return $resource(
            './api/organizations/:organizationId/members/:memberId/locks',
            { organizationId: '@organizationId', memberId: '@memberId'});
    })
    .factory('relationships', function($resource) {
        return $resource(
            './api/organizations/:organizationId/relationship');
    })
    
    
    
;

