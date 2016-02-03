'use strict';

require('angular');
require('angular-route');
require('angular-aria');
require('angular-animate');
require('angular-material');
require('../../node_modules/oidc-token-manager/dist/oidc-token-manager.js');

angular
    .module('CartoonsApp', ['ngMaterial', 'ngRoute']);

require('./routing.config.js');
require('./theme.config.js');

require('./shared/services');
require('./top-bar');
require('./cartoons');
require('./callback');
require('./characters');
require('./token-information');
