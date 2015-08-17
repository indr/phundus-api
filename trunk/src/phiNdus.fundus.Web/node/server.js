/**
 * Main application file
 */

'use strict';

// Set default node environment to development
process.env.NODE_ENV = process.env.NODE_ENV || 'development';

var config = require('./config/environment');

// Bootstrap server
var app = require('koa')();
require('./config/koa')(app);
require('./config/routes')(app);

/*app.use(function*(next) {
  this.status = 200;
  this.body = {msg: "You've reached the default handler..."
    + '\nthis.request.url: ' + this.request.url
    + '\nthis.request.originalUrl: ' + this.request.originalUrl
    + '\nthis.request.href: ' + this.request.href };
});*/

// Start server
if (!module.parent || process.env.NODE_ENV == 'production') {
	app.listen(config.port, config.ip, function () {
  	console.log('Koa server listening on %d, in %s mode', config.port, config.env);
	});
}

// Expose app
//exports = module.exports = app;
module.exports = app;
