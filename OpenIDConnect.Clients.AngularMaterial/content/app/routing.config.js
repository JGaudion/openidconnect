'use strict';

var routeConfig = function ($routeProvider) {
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
        })
        .when('/tokenInformation', {
            templateUrl: '/html/TokenInformation.html',
            controller: 'TokenInformationController'
        });
};

angular.module('CartoonsApp').config(routeConfig);

module.exports = routeConfig;
