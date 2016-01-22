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
            authority: 'https://localhost:44333/core',
            client_id: 'angular14',
            redirect_uri: 'https://localhost:44303/callback',
            post_logout_redirect_uri: 'https://localhost:44303',
            response_type: 'id_token token',
            scope: 'openid profile api'
        };

        return new OidcTokenManager(settings);
    }
  }
})(OidcTokenManager);