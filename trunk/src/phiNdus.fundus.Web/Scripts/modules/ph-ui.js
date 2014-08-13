
angular.module('ph.ui', [])

    .directive('phHistoryBack', function ($window) {
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
; // ph.ui