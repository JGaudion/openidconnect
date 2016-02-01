(function () {
    'use strict';

    angular
        .module('CartoonsApp')
        .controller('CallbackController', CallbackController);

    CallbackController.$inject = ['OidcService', '$routeParams', '$location'];

    function CallbackController(OidcService, $routeParams, $location) {
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
})();
