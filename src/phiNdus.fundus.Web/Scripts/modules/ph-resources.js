angular.module('ph.resources', ['ngResource'])

    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.responseInterceptors.push('errorMessageHttpInterceptor');
        $httpProvider.defaults.headers.patch = {
            'Content-Type': 'application/json;charset=utf-8'
        }
    }])

    .factory('errorMessageHttpInterceptor', function ($q) {
        return function (promise) {
            return promise.then(function (response) {
                return response;
            }, function (response) {

                // TODO: Separation of concerns

                var headerText = "Unbekannter Fehler";
                var bodyText = "<p>Es ist ein unbekannter Fehler aufgetreten.</p><p>Bitte versuchen Sie es später noch einmal.</p>"; 

                if (response.status == 0) {
                    headerText = "Netzwerkfehler";
                    bodyText = "<p>Der Server konnte nicht erreicht werden oder hat nicht in der erwarteten Zeit geantwortet.</p>"
                        + "<p>Kontrollieren Sie Ihre Netzwerkverbindung oder versuchen Sie es zu einem späteren Zeitpunkt nochmal.</p>";
                }
                else if (response.status > 0) {
                    headerText = "Fehler";
                    if (response.data.exceptionMessage != undefined)
                        bodyText = "<p>" + response.data.exceptionMessage + "</p>";
                    else
                        bodyText = "<p>" + response.data.message + "</p>";
                }

                var $div = $('#modal-show-error');
                $div.find('.modal-header h3').html(headerText);
                $div.find('.modal-body').html(bodyText);
                $div.modal();

                return $q.reject(response);
            });
        }
    })

    .factory('members', function($resource) {
        var member = $resource(
            './api/organizations/:organizationId/members/:id/:action',
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
            './api/organizations/:organizationId/relationships');
    })
    .factory('organizationOrders', function($resource) {
        return $resource(
            './api/organizations/:organizationId/orders/:orderId',
            { organizationId: '@organizationId', orderId: '@orderId' },
            { update: { method: 'PATCH' } });

    })
    .factory('organizationOrderItems', function ($resource) {
        return $resource(
            './api/organizations/:organizationId/orders/:orderId/items/:orderItemId',
            { organizationId: '@organizationId', orderId: '@orderId', orderItemId: '@orderItemId' },
            { update: { method: 'PATCH' } });

    })
    .factory('organizationContracts', function($resource) {
        return $resource(
            './api/organizations/:organizationId/contracts/:contractId',
            { organizationId: '@organizationId', contractId: '@contractId' });
    })
    
    .factory('orders', function($resource) {
        return $resource('./api/orders/:orderId');
    })
    
    .factory('contracts', function($resource) {
        return $resource('./api/contracts/:contractId');
    })
    
; // ph.resources

