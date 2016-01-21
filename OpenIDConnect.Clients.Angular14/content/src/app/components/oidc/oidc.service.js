(function (OidcTokenManager) {
  'use strict';

  angular
    .module('src')
    .factory('oidc', oidc);

  /** @ngInject */
  function oidc($log, $http) {
    
    var service = {
      get: get      
    };

    return service;



    var mgr;

    function get() {
        if (!mgr) {
            mgr = createTokenManager();
        }

        return mgr;
    }

    function createTokenManager() {
        var settings = {
            authority: 'https://localhost:44302/core',
            client_id: 'angular14',
            redirect_uri: 'https://localhost:44303/callback',
            response_type: 'id_token token',
            scope: 'openid api'
        };

        return new OidcTokenManager(settings);
    }
  }
})(OidcTokenManager);
