(function () {
    'use strict';

angular.module('CartoonsApp')
.controller('CartoonController', CartoonController);

    CartoonController.$inject = ['$http', 'CartoonsService'];
    function CartoonController($http, CartoonsService) {
    var ctrl = this;

        CartoonsService.get().then(function (data) {
            ctrl.cartoons = data;
        });
    };
})();