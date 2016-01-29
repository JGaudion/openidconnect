(function () {
    'use strict';

    angular
        .module('CartoonsApp')
        .factory('OidcService', OidcService);

    function OidcService() {
        var self = this;

        var settings = {
            authority: 'https://localhost:44333/core',
            client_id: 'angularMaterial',
            redirect_uri: 'https://localhost:44300/#/callback/',
            post_logout_redirect_uri: 'https://localhost:44300/',
            response_type: 'id_token token',
            scope: 'openid profile api'
        };
        
        self.tokenManager = new OidcTokenManager(settings);

        return self;
    };
})();