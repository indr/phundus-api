'use strict';

var controller = require('./status.controller');
var router = require('koa-router')();

router.get('/', controller.index);
module.exports = router.routes();