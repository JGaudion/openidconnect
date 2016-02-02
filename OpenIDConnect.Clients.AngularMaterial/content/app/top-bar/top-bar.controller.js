'use strict';

var TopBarController = function (OidcService) {
    var vm = this;

    vm.login = function () {
        OidcService.tokenManager.redirectForToken();
    }

    vm.logout = function () {
        OidcService.tokenManager.redirectForLogout();
    }

    return vm;
}

angular
    .module('CartoonsApp')
    .controller('TopBarController', TopBarController);

module.exports = TopBarController;
