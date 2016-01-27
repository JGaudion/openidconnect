//Routing
angular.module('CartoonsApp', ['ngMaterial', 'ngRoute'])
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/login',
            {               
                templateUrl: '/content/html/login.html',
                
            })
        .when('/cartoons', {            
            templateUrl: '/content/html/cartoonslist.html',
            controller: 'CartoonController'
        })
            .when('/home', {
                templateUrl: '/content/html/home.html',
                controller: 'HomeController'
            })
        .otherwise({ redirectTo: '/' });
    }]);

//Theme
angular.module('CartoonsApp')
    .config(function ($mdThemingProvider) {
        $mdThemingProvider.theme('default') //defines this as the default one to use throughout my app
        .primaryPalette('teal') //my main colour scheme
        .accentPalette('indigo',
        {
            'default': '300'
        })//For contrast
    });
