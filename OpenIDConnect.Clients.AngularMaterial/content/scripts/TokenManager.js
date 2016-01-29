(function (OidcTokenManager) {
    'use strict';

    angular.module("CartoonsApp")
        .factory("TokenManager", TokenManager);

    function TokenManager() {
        var vm = this;
        var oauthSettings = {
            response_Type : 'id_token',
            client_id : 'cartoonsApp', //This app's unique reference
            authority : 'https://localhost:44333/core', //base url of the identity server
            redirect_uri : 'https://localhost:44300/callback', //where to send the users after they log in
            scope : 'openid profile', //What is the application asking for
            post_logout_redirect_uri : 'https://localhost:44300/bye', //where to send the users after they logout
            filter_protocol_claims: true //
        };
       
        
        var manager = new OidcTokenManager(oauthSettings);
        //Things like RemoveToken come from this token manager
        
        manager.addOnTokenRemoved = TokenRemoved;
        manager.addOnTokenObtained = TokenObtained;

        function TokenRemoved() {
            console.log("Token removed");
        };

        function TokenObtained() {
            console.log("Token obtained");
        };
       
        return manager;

    }
})(OidcTokenManager);
