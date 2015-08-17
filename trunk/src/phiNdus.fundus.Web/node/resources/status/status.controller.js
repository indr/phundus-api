'use strict';


exports.index = function*(next) {
  this.status = 200;
  this.body = {
    status: 'OK', msg: "You've reached the status handler...",
    request: {
      url: this.request.url,
      originalUrl: this.request.originalUrl,
      href: this.request.href
    }
  }
};
