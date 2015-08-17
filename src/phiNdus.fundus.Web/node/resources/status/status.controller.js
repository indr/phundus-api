'use strict';


exports.index = function*(next) {
  this.status = 200;
  this.body = { msg: "You've reached the status handler..."
    + '\nthis.request.url: ' + this.request.url
    + '\nthis.request.originalUrl: ' + this.request.originalUrl
    + '\nthis.request.href: ' + this.request.href };
};
