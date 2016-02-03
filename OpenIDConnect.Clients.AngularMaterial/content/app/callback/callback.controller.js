'use strict';

var CallbackController = function (OidcService, $routeParams, $location) {
    var self = this;

    var hash = $routeParams.response;
    if (hash.charAt(0) === "&") {
        hash = hash.substr(1);
    }

    var manager = OidcService.tokenManager;

    manager.processTokenCallbackAsync(hash)
        .then(function () {
            $location.url('/');
        });

    return self;
};

angular
    .module('CartoonsApp')
    .controller('CallbackController', CallbackController);

module.exports = CallbackController;
