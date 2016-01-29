(function () {
    'use strict';

    angular.module('CartoonsApp')
      .controller('CallbackController', CallbackController);

    CallbackController.$inject = ["TokenManager", '$location']; //dependencies we need inside the Login Controller
    function CallbackController(TokenManager, $location) {
        start();

        function start() {
            Console.Log("test");
            TokenManager.processTokenCallbackAsync()
                .then(GoHome('/cartoons'), ShowError);

            function GoHome() {
                $location.path(destination);
            }
            function ShowError(error) {
                console.log('There was an error processing the returned OpenID Provider token: ' + error.message || error);
            }
        }
    }
})();