angular.module('phundus-api', ['ngResource'])
    .factory('Member', function ($resource) {
        var member = $resource('api/abcd/members/:id');

        member.prototype.update = function (cb) {
            return member.update({ id: this._id.$oid },
                angular.extend({}, this, { _id: undefined }), cb);
        };

        member.prototype.destroy = function (cb) {
            return member.remove({ id: this._id.$oid }, cb);
        };

        return member;
    })