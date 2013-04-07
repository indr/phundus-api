// This is a module for cloud persistance in mongolab - https://mongolab.com
angular.module('mongolab', ['ngResource']).
    factory('Members', function ($resource) {
        var Member = $resource(
            '../api/:org/members/:id',
            { id: '@id' },
            { update: { method: 'PUT'} }
        );

        

        //        Member.prototype.update = function (cb) {
        //            return Member.update({ id: this.id }, cb);
        //        };

        return Member;
    });