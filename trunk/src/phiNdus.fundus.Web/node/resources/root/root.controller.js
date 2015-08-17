'use strict';


exports.index = function*(next) {
  this.status = 403;
  this.body = {
    name: 'phundus-api',
    info: 'API Docs URL'
  };
};
