(function () {
    'use strict';

    angular
      .module('src')
      .controller('CallbackController', CallbackController);

    /** @ngInject */
    function CallbackController(oidc, $state, $log) {
        var vm = this;

        activate();

        function activate() {
            var mgr = oidc.get();

            mgr.processTokenCallbackAsync().then(function () {
                $state.go("home");
            }, function (error) {
                $log.error('There was an error processing the returned OpenID Provider token: ' + error.message || error);
            });
        };
    }
})();
