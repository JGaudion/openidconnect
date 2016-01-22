(function() {
  'use strict';

  angular
    .module('src')
    .config(routerConfig);

  /** @ngInject */
  function routerConfig($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('main', {
          templateUrl: 'app/main/main.html',
        })
        .state('main.home', {
            url: '/',
            templateUrl: 'app/home/home.html',
            controller: 'HomeController',
            controllerAs: 'vm'
        })
        .state('main.news', {
          url: '/news',
          templateUrl: 'app/news/news.html',
          controller: 'NewsController',
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
