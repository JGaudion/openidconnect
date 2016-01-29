(function () {
    'use strict';

    angular.module("CartoonsApp")
        .controller("WelcomeController", WelcomeController);

    WelcomeController.$inject = [ "TokenManager"]; //dependencies we need inside the Welcome Controller
    function WelcomeController(TokenManager) {
        var vm = this;

        
    }


})();