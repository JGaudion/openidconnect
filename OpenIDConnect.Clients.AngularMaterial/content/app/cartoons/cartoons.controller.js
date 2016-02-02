'use strict';

var CartoonController = function ($http, CartoonsService) {
    var ctrl = this;

    CartoonsService.get().then(function (data) {
        ctrl.cartoons = data;
    });
};

angular
    .module('CartoonsApp')
    .controller('CartoonController', CartoonController);

module.exports = CartoonController;
