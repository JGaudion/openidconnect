(function () {
    'use strict';
    angular.module('CartoonsApp')
        .controller('HomeController', HomeController);

    HomeController.$inject = ["$location"];
    function HomeController($location)
    {
        console.log('home controller');
        this.go = function (destination) {
            console.log("going");
            $location.path(destination);
        }
        this.test = function () {
            console.log("tset");
        }
    }
 })();

