(function() {
  'use strict';

  angular.module('src')
    .factory('api', api);

  function api($http, oidc) {
    var service = {
      get: get
    };

    return service;

    function get(route) {
      console.log(oidc.get().access_token);
      return $http( {
        method: 'GET',
        url: 'api/' + route,
        headers: {
          "Authorization": "Bearer " + oidc.get().access_token
        }
      });
    }

  }
})();
