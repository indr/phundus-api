angular
    .module('phundus-api', ['ngResource'])
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
    });