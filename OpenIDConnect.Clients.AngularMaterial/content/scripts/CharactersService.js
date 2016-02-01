(function(){
    'use strict';

    angular
        .module('CartoonsApp')
        .factory('CharactersService', CharactersService);

    CharactersService.$inject = ['$http', 'OidcService'];

    function CharactersService($http, OidcService) {
        var self = this;

        self.get = function () {
            return $http(
                {
                    method: 'GET',
                    url: '/api/characters',
                    headers: { 'Authorization': 'Bearer ' + OidcService.tokenManager.access_token }
                })
                .then(function (response) {
                    return response.data;
                });
            }

            return self;
    };
})();
