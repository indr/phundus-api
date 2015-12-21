
angular.module('ph.management', ['ph.resources', 'ph.domain', 'ph.ui', 'ui', 'ui.bootstrap'])
    .filter('replace', function() {
        return function(input, pattern, replace) {
            if (input == null)
                return null;
            return input.replace(new RegExp(pattern, 'mg'), replace);
        };
    })
    .config(function($routeProvider) {
        $routeProvider
            .when('/members', { controller: MembersCtrl, templateUrl: '/Content/Views/Management/Members.html' })
            .when('/applications', { controller: ApplicationsCtrl, templateUrl: '/Content/Views/Management/Applications.html' })
            .when('/orders', { controller: ManagementOrdersCtrl, templateUrl: '/Content/Views/Management/Orders.html' })
            .when('/orders/:orderId', { controller: ManagementOrderCtrl, templateUrl: '/Content/Views/Management/Order.html' })
            .when('/contracts', { controller: ManagementContractsCtrl, templateUrl: '/Content/Views/Management/Contracts.html' })
            .when('/contracts/:contractId', { controller: ManagementContractCtrl, templateUrl: '/Content/Views/Management/Contract.html' })
            .when('/settings', { controller: SettingsCtrl, templateUrl: '/Content/Views/Management/Settings.html' })
            .when('/files', { controller: FilesCtrl, templateUrl: '/Content/Views/Management/Files.html' });
    })
; // ph.management

function ManagementOrdersCtrl($scope, $location, organizationOrders) {
    $scope.isLoading = true;
    $scope.orders = organizationOrders.query({ "organizationId": $scope.organizationId }, function () { $scope.isLoading = false; }, function () { $scope.isLoading = false; });

    $scope.createOrder = function() {
        $scope.newOrder = { userId: '', organizationId: $scope.organizationId };
        $('#modal-createOrder').modal('show');
    };

    $scope.createOrderOk = function (newOrder) {
        organizationOrders.save(newOrder, function (data) {
            $('#modal-createOrder').modal('hide');
            $scope.openOrder(data);
        }, function () {
            $('#modal-createOrder').modal('show');
        });
    };

    $scope.openOrder = function(order) {
        $location.path('/orders/' + order.orderId);
    };

    $scope.search = { status: '' };
    $scope.order = '-createdUtc';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };
};

function ManagementOrderCtrl($scope, $location, $routeParams, organizationOrders, organizationOrderItems) {
    $scope.isLoading = true;
    $scope.isLoaded = false;
    $scope.order = organizationOrders.get({ "organizationId": $scope.organizationId, "orderId": $routeParams.orderId },
        function() { $scope.isLoading = false; $scope.isLoaded = true; }, function () { $scope.isLoading = false; });

    $scope.newItem = {
        articleId: '', amount: 1, fromUtc: new Date(), toUtc: new Date(),
        organizationId: $scope.organizationId, orderId: $scope.order.orderId
    };

    $scope.showAddItem = function (order) {
        $scope.newItem.orderId = order.orderId;
        $scope.newItem.articleId = '';
        $scope.newItem.amont = 1;
        $('#modal-add-item').modal('show');
    };

    $scope.addItem = function (item) {
        
        organizationOrderItems.save(item, function(data) {
            $('#modal-add-item').modal('hide');
            $scope.order.items.push(data);
        }, function() {
            $('#modal-add-item').modal('show');
        });
    };

    $scope.editItem = function(item) {
        $scope.saveValues = {
            amount: item.amount,
            fromUtc: item.fromUtc,
            toUtc: item.toUtc,
            itemTotal: item.itemTotal
        };

        item.editing = true;

        // TODO: Timezone issue
        //var from = moment(new Date(item.fromUtc)).format("YYYY-MM-DDTHH:mm:ss") + "+00:00";
        //from = new Date(from);
        //var to = moment(new Date(item.toUtc)).format("YYYY-MM-DDTHH:mm:ss") + "+00:00";
        //to = new Date(to);
        //item.fromUtc = from;
        //item.toUtc = to;
    };

    $scope.calculateItemTotal = function (item) {
        var days = Math.max(1, Math.ceil((new Date(item.toUtc) - new Date(item.fromUtc)) / (1000 * 60 * 60 * 24)));

        item.itemTotal = Math.round(100 * item.unitPrice / 7 * days * item.amount) / 100;
    };

    $scope.saveEditedItem = function(item) {
        item.editing = false;
        organizationOrderItems.update({ organizationId: $scope.organizationId, orderId: $scope.order.orderId }, item, function (data) {

            // $scope.order.items replace?
            item.amount = data.amount;
            item.fromUtc = data.fromUtc;
            item.toUtc = data.toUtc;
            item.isAvailable = data.isAvailable;
            item.unitPrice = data.unitPrice;
            item.itemTotal = data.itemTotal;
        });
    };

    $scope.cancelEditing = function(item) {
        item.editing = false;
        item.amount = $scope.saveValues.amount;
        item.fromUtc = $scope.saveValues.fromUtc;
        item.toUtc = $scope.saveValues.toUtc;
        item.itemTotal = $scope.saveValues.itemTotal;
    };

    $scope.removeItem = function (item) {
        if (!confirm('Möchten Sie die Position "' + item.text + '" wirklich löschen?'))
            return;

        organizationOrderItems.delete({ organizationId: $scope.organizationId, orderId: $scope.order.orderId, orderItemId: item.orderItemId }, function (data) {
            var idx = $scope.order.items.indexOf(item);
            $scope.order.items.splice(idx, 1);
        });
    };

    $scope.confirmOrder = function (order) {
        var notAvailableCount = 0;
        for (var i = 0; i < $scope.order.items.length; i++) {
            if (!$scope.order.items[i].isAvailable)
                notAvailableCount++;
        }

        var msg = 'Möchten Sie die Bestellung wirklich bestätigen?';
        if (notAvailableCount == 1)
            msg = '1 Position ist zur Zeit nicht verfügbar.\n\n' + msg;
        else if (notAvailableCount > 1)
            msg = notAvailableCount + ' Positionen sind zur Zeit nicht verfügbar.\n\n' + msg;

        if (!confirm(msg))
            return;

        var status = order.status;
        order.status = 'Approved';
        order.$update(function() {}, function() {
            order.status = status;
        });
    };

    $scope.rejectOrder = function (order) {
        if (!confirm('Möchten Sie die Bestellung wirklich ablehnen?'))
            return;

        var status = order.status;
        order.status = 'Rejected';
        order.$update(function () { }, function () {
            order.status = status;
        });
    };

    $scope.closeOrder = function (order) {
        if (!confirm('Möchten Sie die Bestellung wirklich abschliessen?\n\nAusstehendes Material wird dann nicht mehr reserviert sein.'))
            return;

        var status = order.status;
        order.status = 'Closed';
        order.$update(function () { }, function () {
            order.status = status;
        });
    };

    $scope.getTotal = function () {
        if ($scope.order.items == undefined)
            return 0;

        var total = 0;
        
        for (var i = 0; i < $scope.order.items.length; i++) {
            total += parseFloat($scope.order.items[i].itemTotal);
        }
        return total;
    }
};

function ManagementContractsCtrl($scope, $location, organizationContracts) {
    $scope.isLoading = true;
    $scope.contracts = organizationContracts.query({ "organizationId": $scope.organizationId }, function() { $scope.isLoading = false; }, function() { $scope.isLoading = false; });

    $scope.openContract = function(contract) {
        $location.path('/contracts/' + contract.contractId);
    };

    $scope.search = { };
    $scope.order = '-createdUtc';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };
};

function ManagementContractCtrl($scope, $location, $routeParams, organizationContracts) {
    $scope.isLoading = true;
    $scope.isLoaded = false;

    $scope.contract = organizationContracts.get({ "organizationId": $scope.organizationId, "contractId": $routeParams.contractId },
        function () { $scope.isLoading = false; $scope.isLoaded = true; }, function () { $scope.isLoading = false; });

    $scope.signContract = function(contract) {
        alert('tbd');
    };

    $scope.rescindContract = function (contract) {
        alert('tbd');
    };
};

function DebugCtrl($scope, $route, $routeParams, $location) {
    $scope.$route = $route;
    $scope.$location = $location;
    $scope.$routeParams = $routeParams;

};

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
            });
    };

}

function ApplicationsCtrl($scope, applications, members) {
    $scope.isLoading = true;
    $scope.applications = applications.query({ organizationId: $scope.organizationId }, function() { $scope.isLoading = false; }, function() { $scope.isLoading = false; });
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
                application.isApproved = true;
            });
        }
    };

    $scope.reject = function(application) {
        if (!confirm('Möchten Sie "' + application.firstName + ' ' + application.lastName + '" wirklich ablehnen?'))
            return;

        application.$delete(function () { application.isRejected = true; });
    };
}

function MembersCtrl($scope, $location, members, membersLocks, organizationContracts) {
    $scope.isLoading = true;
    $scope.members = members.query({ organizationId: $scope.organizationId }, function () { $scope.isLoading = false; }, function () { $scope.isLoading = false; });

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
            member.$update({ organizationId: $scope.organizationId },
            function (data, putResponseHeaders) {
                member.role = roleValue;
            });
        }
    };

    $scope.approve = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich bestätigen?')) {
            member.$update({ organizationId: $scope.organizationId, action: 'approve' },
            function (data, putResponseHeaders) {
                member.isApproved = true;
            });
        }
    };

    $scope.lock = function (member) {
        if (confirm('Möchen Sie "' + member.firstName + ' ' + member.lastName + '" wirklich sperren?')) {
            membersLocks.save({ organizationId: $scope.organizationId, memberId: member.id },
            function (data, putResponseHeaders) {
                member.isLocked = true;
            });
        }
    };

    $scope.unlock = function (member) {
        if (confirm('Möchten Sie "' + member.firstName + ' ' + member.lastName + '" wirklich entsperren?')) {
            membersLocks.delete({organizationId: $scope.organizationId, memberId: member.id},
            function (data, putResponseHeaders) {
                member.isLocked = false;
            });
        }
    };

    $scope.createContract = function (member) {
        organizationContracts.save({ organizationId: $scope.organizationId }, { userId: member.id }, function (value, responseHeaders) {
            $location.path('/contracts/' + value.contractId);
        });
    };
}