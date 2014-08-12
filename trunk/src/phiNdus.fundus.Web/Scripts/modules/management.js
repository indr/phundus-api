
angular.module('management', ['phundus-api', 'ui', 'ui.bootstrap'])

    .filter('replace', function () {
        return function(input, pattern, replace) {
            if (input == null)
                return null;
            return input.replace(new RegExp(pattern, 'mg'), replace);
        };
    })
    .filter('orderStatusText', function() {
        return function(input) {
            return {
                "Pending": "Provisorisch", "Approved": "Bestätigt", "Rejected": "Abgelehnt", "Closed": "Abgeschlossen"
            }
            [input];
        };
    })
    .config(function($routeProvider) {
        $routeProvider
            .when('/members', { controller: MembersCtrl, templateUrl: './Content/Views/Management/Members.html' })
            .when('/applications', { controller: ApplicationsCtrl, templateUrl: './Content/Views/Management/Applications.html' })

            .when('/orders', { controller: OrdersCtrl, templateUrl: './Content/Views/Management/Orders.html' })
            .when('/orders/:orderId', {controller: OrderCtrl, templateUrl: './Content/Views/Management/Order.html' })
            .when('/contracts', { controller: ContractsCtrl, templateUrl: './Content/Views/Management/Contracts.html' })
            .when('/contracts/:contractId', { controller: ContractCtrl, templateUrl: './Content/Views/Management/Contract.html' })
            
            .when('/settings', { controller: SettingsCtrl, templateUrl: './Content/Views/Management/Settings.html' })
            .when('/files', { controller: FilesCtrl, templateUrl: './Content/Views/Management/Files.html' });
    })
    .directive('phHistoryBack', function($window) {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                elem.bind('click', function () {
                    $window.history.back();
                });
            }
        }
    })
    .directive('bsDatefield', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                var dateFormat = attrs.bsDatefield || 'DD.MM.YYYY';
                ngModelCtrl.$parsers.push(function (viewValue) {
                    //convert string input into moment data model
                    var parsedMoment = moment(viewValue, dateFormat);
                    //toggle validity
                    ngModelCtrl.$setValidity('datefield', parsedMoment.isValid());
                    //return model value
                    return parsedMoment.isValid() ? parsedMoment.toDate() : undefined;
                });
                ngModelCtrl.$formatters.push(function (modelValue) {
                    var isModelADate = angular.isDate(modelValue);
                    ngModelCtrl.$setValidity('datefield', isModelADate);
                    return isModelADate ? moment(modelValue).format(dateFormat) : undefined;
                });
            }
        };
    })
;

function OrdersCtrl($scope, $location, orders) {
    $scope.orders = orders.query({ "organizationId": $scope.organizationId });

    $scope.createOrder = function() {
        $scope.newOrder = { userId: '', organizationId: $scope.organizationId };
        $('#modal-createOrder').modal('show');
    };

    $scope.createOrderOk = function (newOrder) {
        orders.save(newOrder, function (data) {
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
    $scope.order = '-createdOn';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };
};

function OrderCtrl($scope, $routeParams, orders, orderItems) {
    $scope.order = orders.get({ "organizationId": $scope.organizationId, "orderId": $routeParams.orderId });

    $scope.printPdf = function (order) {
        alert('tbd');
    };
        
    $scope.newItem = {
        articleId: '', amount: 1, from: new Date(), to: new Date(),
        organizationId: $scope.organizationId, orderId: $scope.order.orderId
    };

    $scope.showAddItem = function (order) {
        $scope.newItem.orderId = order.orderId;
        $scope.newItem.articleId = '';
        $scope.newItem.amont = 1;
        $('#modal-add-item').modal('show');
    };

    $scope.addItem = function (item) {
        
        orderItems.save(item, function(data) {
            $('#modal-add-item').modal('hide');
            $scope.order.items.push(data);
        }, function() {
            $('#modal-add-item').modal('show');
        });
    };

    $scope.editItem = function(item) {
        $scope.saveValues = {
            amount: item.amount,
            from: item.from,
            to: item.to
        };

        item.editing = true;

        // TODO: Timezone issue
        var from = moment(new Date(item.from)).format("YYYY-MM-DDTHH:mm:ss") + "+00:00";
        from = new Date(from);
        var to = moment(new Date(item.to)).format("YYYY-MM-DDTHH:mm:ss") + "+00:00";
        to = new Date(to);
        item.from = from;
        item.to = to;
    };

    $scope.saveEditedItem = function(item) {
        item.editing = false;
        orderItems.update({ organizationId: $scope.organizationId, orderId: $scope.order.orderId }, item, function (data) {

            // $scope.order.items replace?
            item.amount = data.amount;
            item.from = data.from;
            item.to = data.to;
            item.isAvailable = data.isAvailable;
            item.unitPrice = data.unitPrice;
            item.itemTotal = data.itemTotal;
        });
    };

    $scope.cancelEditing = function(item) {
        item.editing = false;
        item.amount = $scope.saveValues.amount;
        item.from = $scope.saveValues.from;
        item.to = $scope.saveValues.to;
    };

    $scope.removeItem = function (item) {
        if (!confirm('Möchten Sie die Position "' + item.text + '" wirklich löschen?'))
            return;

        orderItems.delete({ organizationId: $scope.organizationId, orderId: $scope.order.orderId, orderItemId: item.orderItemId }, function (data) {
            var idx = $scope.order.items.indexOf(item);
            $scope.order.items.splice(idx, 1);
        });
    };

    $scope.confirmOrder = function (order) {
        alert('tbd');
    };

    $scope.rejectOrder = function (order) {
        alert('tbd');
    };

    $scope.closeOrder = function (order) {
        alert('tbd');
    };

    $scope.getTotal = function () {
        if ($scope.order.items == undefined)
            return 0;

        var total = 0;
        
        for (var i = 0; i < $scope.order.items.length; i++) {
            total += $scope.order.items[i].itemTotal;
        }
        return total;
    }
};

function ContractsCtrl($scope, $location, contracts) {
    $scope.contracts = contracts.query({ "organizationId": $scope.organizationId });

    $scope.openContract = function(contract) {
        $location.path('/contracts/' + contract.contractId);
    };

    $scope.search = { };
    $scope.order = '-createdOn';
    $scope.orderBy = function (by) {
        if ($scope.order == by)
            $scope.order = '-' + by;
        else
            $scope.order = by;
    };
};

function ContractCtrl($scope, $location, $routeParams, contracts) {
    $scope.contract = contracts.get({ "organizationId": $scope.organizationId, "contractId": $routeParams.contractId });

    $scope.printPdf = function(contract) {
        alert('tbd');
    };

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

function MembersCtrl($scope, $location, members, membersLocks, contracts) {
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
            member.role = roleValue;
            member.$update({ organizationId: $scope.organizationId, action: 'setrole' },
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
        contracts.save({ organizationId: $scope.organizationId }, { userId: member.id }, function (value, responseHeaders) {
            $location.path('/contracts/' + value.contractId);
        });
    };
}