(function() {
  'use strict';

  angular
    .module('src')
    .config(routerConfig);

  /** @ngInject */
  function routerConfig($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('home', {
            url: '/',
            templateUrl: 'app/main/main.html',
            controller: 'MainController',
            controllerAs: 'vm'
        })
        .state('callback', {
            url: '/callback',
            controller: 'CallbackController',
            controllerAs: 'vm'
        });

    $urlRouterProvider.otherwise('/');
  }

})();
