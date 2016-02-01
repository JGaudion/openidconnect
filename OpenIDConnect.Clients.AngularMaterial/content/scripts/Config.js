/* global angular */
(function () {
    'use strict';

    angular
        .module('CartoonsApp', ['ngMaterial', 'ngRoute']);

    //Routing
    angular
        .module('CartoonsApp')
            .config(['$routeProvider', function ($routeProvider) {
                $routeProvider
                    .when('/cartoons', {
                        templateUrl: '/html/Cartoonslist.html',
                        controller: 'CartoonController'
                    })
                    .when('/callback', {
                        url: '/callback',
                        controller : 'CallbackController'
                    })
                    .when('/callback/:response', {
                        template: ' ',
                        controller: 'CallbackController'
                    })
                    .when('/home',{
                        templateUrl: '/html/home.html'
                    })
                    .when('/characters', {
                        templateUrl: '/html/Characters.html',
                        controller: 'CharactersController'
                    });
        }]);

    //Theme
    angular
        .module('CartoonsApp')
            .config(function ($mdThemingProvider) {
                $mdThemingProvider.theme('default') //defines this as the default one to use throughout my app
                    .primaryPalette('teal') //my main colour scheme
                    .accentPalette('indigo',
                    {
                        'default': '300'
                    }); //For contrast
            });
})();
