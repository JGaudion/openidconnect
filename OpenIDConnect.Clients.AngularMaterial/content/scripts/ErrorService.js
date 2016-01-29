(function () {
    'use strict';

    angular.module("CartoonsApp")
        .service("ErrorService", ErrorService);

    ErrorService.$inject = ["$timeout"]; //dependencies we need inside the Error Service
    function ErrorService($timeout) {
        var vm = this;
        
        vm.clear = Clear;
        vm.show = Show;
        
        function Clear() {
            vm.errors = null;
        }

        function Show(err) {
            $timeout(function () {
                if (err instanceof Array) {
                    vm.errors = err;
                }
                else {
                    vm.errors = [err];
                }
            }, 100);
        }
    }
})();
