/* global OidcTokenManager */
(function () {
    angular
        .module('CartoonsApp')
        .controller('TopBarController', TopBarController);

    TopBarController.$inject = ['OidcService'];

    function TopBarController(OidcService) {
        var vm = this;

        vm.login = function () {
            OidcService.tokenManager.redirectForToken();
        }

        vm.logout = function () {
            OidcService.tokenManager.redirectForLogout();
        }

        return vm;
    }
})();