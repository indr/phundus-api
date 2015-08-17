/**
 * Main application routes
 */

'use strict';

var mount = require('koa-mount');

module.exports = function(app) {
  
  // I don't know how to prefix routes with koa-router and koa-mount
  var prefix = '/node';

	// YEOMAN INJECT ROUTES BELOW
	app.use(mount(prefix + '/status', require('../resources/status')));
  app.use(mount(prefix + '/', require('../resources/root')));



};
