(function() {
  'use strict';

  angular.module('src')
    .factory('api', api);

  function api($http) {
    var service = {
      get: get
    };

    return service;

    function get(route) {
      return $http( {
        method: 'GET',
        url: 'https://localhost:44303/api/' + route
      });
    }

  }
})();
