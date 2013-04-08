angular
    .module('phundus-api', ['ngResource'])
    .factory('members', function ($resource) {
        var members = $resource(
            '../api/:org/members/:id',
            { id: '@id' },
            { update: { method: 'PUT'} }
        );
        return members;
    });