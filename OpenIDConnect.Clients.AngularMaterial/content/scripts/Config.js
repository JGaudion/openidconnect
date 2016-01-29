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
                        templateUrl: '/html/cartoonslist.html',
                        controller: 'CartoonController'
                    })
                    .when('/home', {
                        templateUrl: '/html/home.html',
                        controller: 'HomeController',
                        controllerAs : "ctrl"
                    })
                    .when('/callback', {
                        url: '/callback',
                        controller : 'CallbackController'
                    })
                    .when('/bye', {
                        templateUrl: '/html/bye.html'
                    })
                    .when('/welcome', {
                        templateUrl: '/html/welcome.html',
                        controller : 'WelcomeController'
                    })
                    .otherwise({
                        templateUrl: '/html/home.html',
                        controller: 'HomeController'
                    });
            }
            ]);

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