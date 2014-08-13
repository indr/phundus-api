
angular.module('ph.domain', [])
    .filter('orderStatusText', function () {
        return function (input) {
            return {
                "Pending": "Provisorisch", "Approved": "Bestätigt", "Rejected": "Abgelehnt", "Closed": "Abgeschlossen"
            }
            [input];
        };
    })
; // ph.domain