angular
    .module('phundus-api', ['ngResource'])

    .factory('members', function ($resource) {
        var member = $resource(
        // TODO: Url?
            './api/:org/members/:id',
            { id: '@id' },
            { update: { method: 'PUT'} }
        );

        return member;
    })

    .factory('organizations', function ($resource) {
        var organization = $resource(
            './api/organizations/:id',
            { id: '@id'},
            { update: { method: 'PUT' } }
        );

        return organization;
    });