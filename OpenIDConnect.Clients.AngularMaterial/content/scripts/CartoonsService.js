(function () {
    'use strict';

    angular
        .module('CartoonsApp')
        .factory('CartoonsService', CartoonsService);

    CartoonsService.$inject = ['$http', 'OidcService'];

    function CartoonsService($http, OidcService) {
        var self = this;

        self.get = function () {
            return $http(
                {
                    method: 'GET',
                    url: '/api/cartoons',
                    headers: { 'Authorization': 'Bearer ' + OidcService.tokenManager.access_token }
                }).then(function (response) {
                    return response.data;
                });
        }

        return self;
    };

})();