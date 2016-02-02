(function(){
    'use strict';

    angular
        .module('CartoonsApp')
        .factory('TokenInformationService', TokenInformationService);

        TokenInformationService.$inject = ['$http', 'OidcService'];

        function TokenInformationService($http, OidcService){
                var self = this;

                self.get = function () {
                    return $http(
                        {
                            method: 'GET',
                            url: '/api/info',
                            headers: { 'Authorization': 'Bearer ' + OidcService.tokenManager.access_token }
                        }).then(function (response) {
                            return response.data;
                        });
                }

                return self;
        }
})();
