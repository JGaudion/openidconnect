(function () {
    'use strict';
    angular.module('CartoonsApp')
        .controller('CartoonController', CartoonController);

    CartoonController.$inject = ['$http'];
    function CartoonController($http) {
        var ctrl = this;


        $http({ method: 'GET', url: '/api/cartoons' })
            .success(function (data) {
                ctrl.cartoons = data;
                console.log(data);
            });

   
    }
})();